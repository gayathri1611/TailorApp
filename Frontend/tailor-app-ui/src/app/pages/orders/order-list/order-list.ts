import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { OrderService } from '../../../services/order.service';
import { Order } from '../../../models/order';

@Component({
  selector: 'app-order-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
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

  statuses = ['', 'Pending', 'InProgress', 'ReadyForDelivery', 'Delivered', 'Cancelled'];

  constructor(
    private orderService: OrderService,
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
  }

  getStatusColor(status: string): string {
    const map: Record<string, string> = {
      'Pending':          '#F5C518',
      'InProgress':       '#3B82F6',
      'ReadyForDelivery': '#22C55E',
      'Delivered':        '#888888',
      'Cancelled':        '#EF4444'
    };
    return map[status] ?? '#888888';
  }

  getStatusLabel(status: string): string {
    const map: Record<string, string> = {
      'Pending':          'Pending',
      'InProgress':       'In Progress',
      'ReadyForDelivery': 'Ready ✓',
      'Delivered':        'Delivered',
      'Cancelled':        'Cancelled'
    };
    return map[status] ?? status;
  }

  delete(id: number): void {
    if (!confirm('Delete this order?')) return;
    this.orderService.delete(id).subscribe({
      next: () => this.load(),
      error: () => { this.error = 'Delete failed.'; this.cdr.detectChanges(); }
    });
  }
}