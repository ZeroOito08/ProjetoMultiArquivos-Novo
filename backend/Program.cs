var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // <-- Este método agora será reconhecido

// --- ADICIONE ESTA SEÇÃO PARA CONFIGURAR O CORS ---
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
// --- FIM DA SEÇÃO CORS ---

var app = builder.Build();

builder.WebHost.UseUrls("https://localhost:7060", "http://localhost:5178");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // <-- Este método agora será reconhecido
    app.UseSwaggerUI(); // <-- Este método agora será reconhecido
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();