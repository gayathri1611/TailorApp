import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { UserService } from '../../services/user.service';
import { AppUser } from '../../models/auth';
import { Paginator } from '../../shared/paginator/paginator';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule, RouterModule, Paginator],
  templateUrl: './user-list.html',
  styleUrls: ['./user-list.css']
})
export class UserList implements OnInit {
  users: AppUser[] = [];
  loading = false;
  error = '';
  success = '';

  currentPage = 1;
  pageSize = 10;

  get paged(): AppUser[] {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.users.slice(start, start + this.pageSize);
  }

  constructor(
    private userService: UserService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.userService.getAll().subscribe({
      next: (data: AppUser[]) => {
        this.users = data;
        this.currentPage = 1;
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
    this.userService.deactivate(id).subscribe({
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
