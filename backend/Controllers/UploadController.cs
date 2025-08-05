using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System;

[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly IWebHostEnvironment _hostingEnvironment;

    public UploadController(IWebHostEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] List<IFormFile> files)
    {
        if (files == null || files.Count == 0)
        {
            return BadRequest("Nenhum arquivo enviado.");
        }

        long totalSize = files.Sum(f => f.Length);

        // --- CORREÇÃO AQUI para garantir um WebRootPath válido ---
        var webRootPath = _hostingEnvironment.WebRootPath;
        if (string.IsNullOrEmpty(webRootPath))
        {
            // Se WebRootPath for nulo (ex: wwwroot não existe ou não está configurado),
            // use ContentRootPath e tente criar wwwroot
            webRootPath = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot");
            if (!Directory.Exists(webRootPath))
            {
                Directory.CreateDirectory(webRootPath);
            }
        }

        var uploadsFolder = Path.Combine(webRootPath, "uploads"); // <-- LINHA 35 AGORA USA webRootPath GARANTIDO

        // Crie o diretório 'uploads' dentro de 'wwwroot' se ele não existir
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        // --- Validação de Segurança (MUITO IMPORTANTE!) ---
        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".pdf" };
        const long maxFileSize = 10 * 1024 * 1024; // 10 MB por arquivo

        var uploadedFilesInfo = new List<object>();
        var rejectedFilesInfo = new List<object>();

        foreach (var formFile in files)
        {
            var originalFileName = Path.GetFileName(formFile.FileName);
            var fileExtension = Path.GetExtension(originalFileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
            {
                rejectedFilesInfo.Add(new { FileName = originalFileName, Reason = "Tipo de arquivo não permitido." });
                continue;
            }

            if (formFile.Length == 0)
            {
                rejectedFilesInfo.Add(new { FileName = originalFileName, Reason = "Arquivo vazio." });
                continue;
            }
            if (formFile.Length > maxFileSize)
            {
                rejectedFilesInfo.Add(new { FileName = originalFileName, Reason = $"Tamanho do arquivo excede o limite ({maxFileSize / (1024 * 1024)}MB)." });
                continue;
            }

            var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }
                uploadedFilesInfo.Add(new { OriginalFileName = originalFileName, StoredFileName = uniqueFileName, Size = formFile.Length });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar o arquivo {originalFileName}: {ex.Message}");
                rejectedFilesInfo.Add(new { FileName = originalFileName, Reason = $"Erro interno ao salvar: {ex.Message}" });
            }
        }

        return Ok(new
        {
            UploadedCount = uploadedFilesInfo.Count,
            RejectedCount = rejectedFilesInfo.Count,
            TotalSize = totalSize,
            UploadedFiles = uploadedFilesInfo,
            RejectedFiles = rejectedFilesInfo,
            Message = "Processamento de arquivos concluído."
        });
    }
}