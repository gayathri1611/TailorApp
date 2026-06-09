import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { OrderService } from '../../../services/order.service';
import { Order } from '../../../models/order';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.css']
})
export class Dashboard implements OnInit {
  allOrders: Order[] = [];
  loading = false;
  today = new Date();

  constructor(
    private orderService: OrderService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loading = true;
    this.orderService.getAll().subscribe({
      next: (data: Order[]) => {
        this.allOrders = data;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  // Widget 1: New orders (created today)
  get newOrders(): Order[] {
    const todayStr = new Date().toDateString();
    return this.allOrders.filter(o =>
      o.createdDate && new Date(o.createdDate).toDateString() === todayStr
    );
  }

  // Widget 2: Orders due today
  get ordersDueToday(): Order[] {
    const todayStr = new Date().toDateString();
    return this.allOrders.filter(o =>
      o.deliveryDate &&
      new Date(o.deliveryDate).toDateString() === todayStr &&
      o.status !== 'Delivered' &&
      o.status !== 'Cancelled'
    );
  }

  // Widget 3: Pending deliveries (ReadyForDelivery)
  get pendingDeliveries(): Order[] {
    return this.allOrders.filter(o => o.status === 'ReadyForDelivery');
  }

  getStatusColor(status: string): string {
    const map: Record<string, string> = {
      'Pending':          '#F5C518',
      'InProgress':       '#3B82F6',
      'ReadyForDelivery': '#22C55E',
      'Delivered':        '#888',
      'Cancelled':        '#EF4444'
    };
    return map[status] ?? '#888';
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
}