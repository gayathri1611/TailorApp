import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { AppUser } from '../../models/auth';

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
  private allUsers: AppUser[] = [];

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.buildForm();

    const id = this.route.snapshot.params['id'];
    this.isEdit = !!id && this.router.url.includes('edit');
    this.userId = id ?? undefined;

    this.loading = true;
    this.userService.getAll().subscribe({
      next: (users: AppUser[]) => {
        this.allUsers = users;
        if (this.isEdit && this.userId) {
          const user = users.find(u => u.id === this.userId);
          if (user) {
            this.form.patchValue({
              firstName: user.firstName,
              lastName:  user.lastName,
              email:     user.email,
              role:      user.role,
              shopId:    user.shopId
            });
            this.form.get('password')?.clearValidators();
            this.form.get('password')?.updateValueAndValidity();
          }
        }
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.error = 'Failed to load users.';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
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
    this.error = '';

    const others = this.allUsers.filter(u => u.id !== this.userId);
    const email     = this.form.value.email?.trim().toLowerCase();
    const firstName = this.form.value.firstName?.trim().toLowerCase();
    const lastName  = this.form.value.lastName?.trim().toLowerCase();

    const dupEmail = others.find(u => u.email.toLowerCase() === email);
    if (dupEmail) {
      this.error = `Email already used by ${dupEmail.firstName} ${dupEmail.lastName}.`;
      return;
    }

    const dupName = others.find(u =>
      u.firstName.trim().toLowerCase() === firstName &&
      u.lastName.trim().toLowerCase() === lastName
    );
    if (dupName) {
      this.error = `A user named "${this.form.value.firstName} ${this.form.value.lastName}" already exists.`;
      return;
    }

    this.saving = true;

    if (this.isEdit && this.userId) {
      const updatePayload = {
        firstName: this.form.value.firstName,
        lastName:  this.form.value.lastName,
        role:      this.form.value.role,
        shopId:    this.form.value.shopId
      };
      this.userService.update(this.userId, updatePayload).subscribe({
        next: () => this.router.navigate(['/users']),
        error: (err) => {
          this.error = err?.error ?? 'Update failed.';
          this.saving = false;
          this.cdr.detectChanges();
        }
      });
    } else {
      this.userService.register(this.form.value).subscribe({
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
