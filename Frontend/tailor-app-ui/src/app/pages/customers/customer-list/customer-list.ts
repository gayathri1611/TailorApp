import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Customer } from '../../../models/customer';
import { CustomerService } from '../../../services/customer.service';
import { Paginator } from '../../../shared/paginator/paginator';

@Component({
  selector: 'app-customer-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, Paginator],
  templateUrl: './customer-list.html',
  styleUrl: './customer-list.css'
})
export class CustomerList implements OnInit {
  customers: Customer[] = [];
  filtered: Customer[] = [];
  searchTerm = '';
  loading = false;
  error = '';

  currentPage = 1;
  pageSize = 10;

  get paged(): Customer[] {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.filtered.slice(start, start + this.pageSize);
  }

  constructor(
    private customerService: CustomerService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.customerService.getCustomers().subscribe({
      next: (data: Customer[]) => {
        this.customers = data;
        this.filtered  = data;
        this.currentPage = 1;
        this.loading   = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.error   = 'Failed to load customers.';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  onSearch(): void {
    const term = this.searchTerm.toLowerCase();
    this.filtered = this.customers.filter(c =>
      c.firstName.toLowerCase().includes(term) ||
      (c.lastName?.toLowerCase().includes(term) ?? false) ||
      c.phoneNumber.includes(term) ||
      (c.customerCode?.toLowerCase().includes(term) ?? false)
    );
    this.currentPage = 1;
  }

  delete(id: number): void {
    if (!confirm('Delete this customer?')) return;
    this.customerService.deleteCustomer(id).subscribe({
      next: () => this.load(),
      error: () => { this.error = 'Delete failed.'; this.cdr.detectChanges(); }
    });
  }
}