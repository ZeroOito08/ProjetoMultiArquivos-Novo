import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
// import { provideRouter } from '@angular/router'; // Remova se não tiver app.routes.ts
import { provideHttpClient } from '@angular/common/http';

// import { routes } from './app.routes'; // Remova se não tiver app.routes.ts

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    // provideRouter(routes), // Remova esta linha se não tiver app.routes.ts
    provideHttpClient() // <-- DEVE ESTAR AQUI
  ]
};