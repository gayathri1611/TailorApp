import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-user-form',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
  templateUrl: './user-form.html',
  styleUrls: ['./user-form.css']
})
export class UserForm implements OnInit {
  form!: FormGroup;
  isEdit = false;
  userId?: string;
  saving = false;
  loading = false;
  error = '';

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.buildForm();

    const id = this.route.snapshot.params['id'];
    this.isEdit = !!id && this.router.url.includes('edit');
    this.userId = id ?? undefined;

    if (this.isEdit && this.userId) {
      this.loading = true;
      this.authService.getUsers().subscribe({
        next: users => {
          const user = users.find(u => u.id === this.userId);
          if (user) {
            this.form.patchValue({
              firstName: user.firstName,
              lastName:  user.lastName,
              email:     user.email,
              role:      user.role,
              shopId:    user.shopId
            });
            // password not required on edit
            this.form.get('password')?.clearValidators();
            this.form.get('password')?.updateValueAndValidity();
          }
          this.loading = false;
          this.cdr.detectChanges();
        },
        error: () => {
          this.error = 'Failed to load user.';
          this.loading = false;
          this.cdr.detectChanges();
        }
      });
    }
  }

  buildForm(): void {
    this.form = this.fb.group({
      firstName: ['', Validators.required],
      lastName:  ['', Validators.required],
      email:     ['', [Validators.required, Validators.email]],
      password:  ['', [Validators.required, Validators.minLength(6)]],
      role:      ['Staff', Validators.required],
      shopId:    [1, Validators.required]
    });
  }

  submit(): void {
    if (this.form.invalid) return;
    this.saving = true;
    this.error = '';

    const payload = this.form.value;

    if (this.isEdit && this.userId) {
      this.authService.updateUser(this.userId, payload).subscribe({
        next: () => this.router.navigate(['/users']),
        error: (err) => {
          this.error = err?.error ?? 'Update failed.';
          this.saving = false;
          this.cdr.detectChanges();
        }
      });
    } else {
      this.authService.register(payload).subscribe({
        next: () => this.router.navigate(['/users']),
        error: (err) => {
          this.error = err?.error ?? 'Registration failed.';
          this.saving = false;
          this.cdr.detectChanges();
        }
      });
    }
  }
}