import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { OrderService } from '../../../services/order.service';
import { OrderStatusService } from '../../../services/order-status.service';
import { Order } from '../../../models/order';
import { Paginator } from '../../../shared/paginator/paginator';

@Component({
  selector: 'app-order-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, Paginator],
  templateUrl: './order-list.html',
  styleUrls: ['./order-list.css']
})
export class OrderList implements OnInit {
  orders: Order[] = [];
  filtered: Order[] = [];
  searchTerm = '';
  statusFilter = '';
  loading = false;
  error = '';

  currentPage = 1;
  pageSize = 10;

  get paged(): Order[] {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.filtered.slice(start, start + this.pageSize);
  }

  get statuses(): string[] { return this.orderStatus.statuses; }

  constructor(
    private orderService: OrderService,
    private orderStatus: OrderStatusService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void { this.load(); }

  load(): void {
    this.loading = true;
    this.orderService.getAll().subscribe({
      next: (data: Order[]) => {
        this.orders = data;
        this.applyFilters();
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.error = 'Failed to load orders.';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  applyFilters(): void {
    const term = this.searchTerm.toLowerCase();
    this.filtered = this.orders.filter(o => {
      const matchSearch = !term ||
        o.customerName?.toLowerCase().includes(term) ||
        o.orderCode?.toLowerCase().includes(term);
      const matchStatus = !this.statusFilter || o.status === this.statusFilter;
      return matchSearch && matchStatus;
    });
    this.currentPage = 1;
  }

  getStatusColor(status: string): string { return this.orderStatus.getColor(status); }
  getStatusLabel(status: string): string { return this.orderStatus.getLabel(status); }

  delete(id: number): void {
    if (!confirm('Delete this order?')) return;
    this.orderService.delete(id).subscribe({
      next: () => this.load(),
      error: () => { this.error = 'Delete failed.'; this.cdr.detectChanges(); }
    });
  }
}
