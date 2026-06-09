import { Component, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router, ActivatedRoute } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class Login {
  form: FormGroup;
  loading  = false;
  error    = '';
  showPass = false;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private authService: AuthService,
    private cdr: ChangeDetectorRef
  ) {
    this.form = this.fb.group({
      email:    ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });

    // Show error from Google callback if any
    const err = this.route.snapshot.queryParams['error'];
    if (err) this.error = 'Google sign-in failed. Please try email login.';
  }

  submit(): void {
    if (this.form.invalid) return;
    this.loading = true;
    this.error   = '';

    this.authService.login(this.form.value).subscribe({
      next: () => this.router.navigate(['/dashboard']),
      error: () => {
        this.error   = 'Invalid email or password.';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  loginWithGoogle(): void {
    window.location.href = this.authService.getGoogleLoginUrl();
  }
}