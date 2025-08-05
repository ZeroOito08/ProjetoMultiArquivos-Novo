import { bootstrapApplication } from '@angular/platform-browser'; // <-- IMPORTE ESTA LINHA!
import { appConfig } from './app/app.config'; // <-- IMPORTE ESTA LINHA!
import { AppComponent } from './app/app.component'; // <-- IMPORTE ESTA LINHA!

bootstrapApplication(AppComponent, appConfig) // <-- Use bootstrapApplication
  .catch((err) => console.error(err)); // <-- Corrigido 'err' para ter tipo 'any' implicitamente