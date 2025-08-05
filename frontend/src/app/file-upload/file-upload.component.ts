import { Component } from '@angular/core';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { CommonModule } from '@angular/common'; // <-- IMPORTE ESTA LINHA!

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css'],
  standalone: true, // <-- DEVE ESTAR AQUI
  imports: [CommonModule] // <-- DEVE ESTAR AQUI (se usar *ngIf, *ngFor, pipes)
})
export class FileUploadComponent {
  selectedFiles: File[] = [];

  constructor(private http: HttpClient) { }

  onFileSelected(event: any) {
    this.selectedFiles = Array.from(event.target.files);
    console.log('Arquivos selecionados:', this.selectedFiles);
  }

  uploadFiles() {
    if (this.selectedFiles.length === 0) {
      alert('Nenhum arquivo selecionado!');
      return;
    }

    const formData = new FormData();
    for (const file of this.selectedFiles) {
      formData.append('files', file, file.name);
    }

    this.http.post('https://localhost:7060/api/upload', formData, { // Verifique a porta!
      reportProgress: true,
      observe: 'events'
    })
    .subscribe({
      next: (event) => {
        if (event.type === HttpEventType.UploadProgress) {
          console.log('Progresso do Upload:', Math.round(100 * event.loaded / (event.total || 1)) + '%');
        } else if (event.type === HttpEventType.Response) {
          console.log('Upload concluído!', event.body);
          alert('Arquivos enviados com sucesso!');
          this.selectedFiles = [];
        }
      },
      error: (error) => {
        console.error('Erro no upload:', error);
        alert('Erro ao enviar arquivos. Verifique o console para mais detalhes.');
      }
    });
  }
}