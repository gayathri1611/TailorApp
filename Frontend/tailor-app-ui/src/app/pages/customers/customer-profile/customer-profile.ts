import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { CustomerService } from '../../../services/customer.service';
import { OrderService } from '../../../services/order.service';
import { MeasurementService } from '../../../services/measurements-service';
import { OrderStatusService } from '../../../services/order-status.service';
import { Customer } from '../../../models/customer';
import { Order } from '../../../models/order';
import { Measurement } from '../../../models/measurement';

interface GarmentSummary {
  type: string;
  count: number;
  totalAmount: number;
  lastDate: string;
}

@Component({
  selector: 'app-customer-profile',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './customer-profile.html',
  styleUrls: ['./customer-profile.css']
})
export class CustomerProfile implements OnInit {

  customerId!: number;
  customer?: Customer;
  orders: Order[] = [];
  measurements: Measurement[] = [];
  garmentSummary: GarmentSummary[] = [];

  activeTab = 'orders';
  loading   = false;
  error     = '';

  constructor(
    private route: ActivatedRoute,
    private customerService: CustomerService,
    private orderService: OrderService,
    private measurementService: MeasurementService,
    private orderStatus: OrderStatusService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.customerId = +this.route.snapshot.params['id'];
    this.load();
  }

  load(): void {
    this.loading = true;

    this.customerService.getCustomer(this.customerId).subscribe({
      next: c => { this.customer = c; this.tryDone(); },
      error: () => { this.error = 'Failed to load customer.'; this.tryDone(); }
    });

    this.orderService.getByCustomer(this.customerId).subscribe({
      next: orders => {
        this.orders = orders.sort((a, b) =>
          (b.createdDate ? new Date(b.createdDate).getTime() : 0) -
          (a.createdDate ? new Date(a.createdDate).getTime() : 0));
        this.buildGarmentSummary();
        this.tryDone();
      },
      error: () => this.tryDone()
    });

    this.measurementService.getMeasurementsByCustomer(this.customerId).subscribe({
      next: m => {
        this.measurements = m.sort((a, b) =>
          new Date(b.createdDate!).getTime() - new Date(a.createdDate!).getTime());
        this.tryDone();
      },
      error: () => this.tryDone()
    });
  }

  private loadCount = 0;
  tryDone(): void {
    this.loadCount++;
    if (this.loadCount >= 3) {
      this.loading = false;
      this.cdr.detectChanges();
    }
  }

  buildGarmentSummary(): void {
    const map = new Map<string, GarmentSummary>();
    for (const o of this.orders) {
      for (const item of o.orderItems) {
        const type = item.garmentType;
        const existing = map.get(type);
        const amount = (item.unitPrice ?? 0) * (item.quantity ?? 1);
        if (existing) {
          existing.count       += item.quantity ?? 1;
          existing.totalAmount += amount;
          const oDate = o.createdDate ? new Date(o.createdDate) : new Date(0);
          if (oDate > new Date(existing.lastDate))
            existing.lastDate = o.createdDate ?? '';
        } else {
          map.set(type, {
            type, count: item.quantity ?? 1,
            totalAmount: amount, lastDate: o.createdDate ?? ''
          });
        }
      }
    }
    this.garmentSummary = [...map.values()]
      .sort((a, b) => b.count - a.count);
  }

  get initials(): string {
    return (this.customer?.firstName?.[0] ?? '') +
           (this.customer?.lastName?.[0]  ?? '');
  }

  get totalOrders(): number { return this.orders.length; }

  get totalGarments(): number {
    return this.orders.reduce((s, o) =>
      s + o.orderItems.reduce((si, i) => si + (i.quantity ?? 1), 0), 0);
  }

  get totalSpent(): number {
    return this.orders.reduce((s, o) =>
      s + (o.finalAmount ?? o.totalAmount ?? 0), 0);
  }

  get pendingOrders(): number {
    return this.orders.filter(o =>
      o.status !== 'Delivered' && o.status !== 'Cancelled').length;
  }

  get customerSince(): string {
    if (!this.orders.length) return '—';
    const first = this.orders[this.orders.length - 1];
    if (!first.createdDate) return '—';
    return new Date(first.createdDate).toLocaleDateString('en-IN', { month: 'short', year: 'numeric' });
  }

  get loyaltyTag(): string {
    if (this.totalOrders >= 10) return 'VIP';
    if (this.totalOrders >= 5)  return 'Regular';
    return 'New';
  }

  get loyaltyColor(): string {
    if (this.totalOrders >= 10) return '#D97706';
    if (this.totalOrders >= 5)  return '#2563EB';
    return '#16A34A';
  }

  get isRegularCustomer(): boolean {
    return this.totalOrders >= 5;
  }

  getStatusColor(status: string | undefined): string { return this.orderStatus.getColor(status); }
  getStatusLabel(status: string | undefined): string { return this.orderStatus.getLabel(status); }
}
