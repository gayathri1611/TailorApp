import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { AppUser } from '../../models/auth';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './user-list.html',
  styleUrls: ['./user-list.css']
})
export class UserList implements OnInit {
  users: AppUser[] = [];
  loading = false;
  error = '';
  success = '';

  constructor(
    private authService: AuthService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.authService.getUsers().subscribe({
      next: (data: AppUser[]) => {
        this.users = data;
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

  deactivate(id: string, name: string): void {
    if (!confirm(`Deactivate ${name}?`)) return;
    this.authService.deactivateUser(id).subscribe({
      next: () => {
        this.success = `${name} deactivated.`;
        this.load();
      },
      error: () => {
        this.error = 'Deactivate failed.';
        this.cdr.detectChanges();
      }
    });
  }

  getRoleBadge(role: string): string {
    return role === 'Admin' ? 'bg-danger' : 'bg-primary';
  }
}