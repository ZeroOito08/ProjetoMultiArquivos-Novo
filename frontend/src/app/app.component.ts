import { Component } from '@angular/core';
import { CommonModule } from '@angular/common'; // Para *ngIf, *ngFor, pipes
import { FileUploadComponent } from './file-upload/file-upload.component'; // Importe seu componente de upload

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  standalone: true, // <-- DEVE ESTAR AQUI
  imports: [
    CommonModule, // <-- DEVE ESTAR AQUI
    FileUploadComponent // <-- DEVE ESTAR AQUI
  ]
})
export class AppComponent {
  title = 'frontend';
}