import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'forgot-password',
    loadComponent: () =>
      import('./auth/forgot-password/forgot-password').then(m => m.ForgotPassword)
  },
  {
    path: 'reset-password',
    loadComponent: () =>
      import('./auth/reset-password/reset-password').then(m => m.ResetPassword)
  },
  { path: '', redirectTo: 'forgot-password', pathMatch: 'full' }
];
