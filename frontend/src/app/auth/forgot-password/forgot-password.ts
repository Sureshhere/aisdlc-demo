import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { PasswordResetService } from '../services/password-reset';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './forgot-password.html',
  styleUrl: './forgot-password.scss',
})
export class ForgotPassword {
  form: FormGroup;
  submitted = false;
  loading = false;
  errorMessage = '';

  constructor(private fb: FormBuilder, private passwordResetService: PasswordResetService) {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  get email() {
    return this.form.get('email')!;
  }

  onSubmit(): void {
    if (this.form.invalid) return;

    this.loading = true;
    this.errorMessage = '';

    this.passwordResetService.requestPasswordReset({ email: this.email.value }).subscribe({
      next: () => {
        this.submitted = true;
        this.loading = false;
      },
      error: () => {
        // Always show the generic success message to prevent email enumeration
        this.submitted = true;
        this.loading = false;
      }
    });
  }
}
