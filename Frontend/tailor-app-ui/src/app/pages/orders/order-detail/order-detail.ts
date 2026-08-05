import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { OrderService } from '../../../services/order.service';
import { MeasurementService } from '../../../services/measurements-service';
import { OrderStatusService } from '../../../services/order-status.service';
import { Order } from '../../../models/order';
import { Measurement } from '../../../models/measurement';

@Component({
  selector: 'app-order-detail',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './order-detail.html',
  styleUrls: ['./order-detail.css']
})
export class OrderDetail implements OnInit {
  order?: Order;
  measurement?: Measurement;
  loading = false;
  error = '';
  orderId!: number;

  measurementFields = [
    { key: 'chest',         label: 'Chest' },
    { key: 'shoulder',      label: 'Shoulder' },
    { key: 'sleeveLength',  label: 'Sleeve' },
    { key: 'waist',         label: 'Waist' },
    { key: 'hip',           label: 'Hip' },
    { key: 'neck',          label: 'Neck' },
    { key: 'armHole',       label: 'Arm Hole' },
    { key: 'thigh',         label: 'Thigh' },
    { key: 'knee',          label: 'Knee' },
    { key: 'inseamLength',  label: 'Inseam' },
    { key: 'outseamLength', label: 'Outseam' },
    { key: 'height',        label: 'Height' },
  ];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private orderService: OrderService,
    private measurementService: MeasurementService,
    private orderStatus: OrderStatusService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.orderId = +this.route.snapshot.params['id'];
    this.loading = true;
    this.orderService.getById(this.orderId).subscribe({
      next: o => {
        this.order = o;
        if (o.measurementId) {
          this.measurementService.getMeasurement(o.measurementId).subscribe({
            next: m => { this.measurement = m; this.cdr.detectChanges(); },
            error: () => { this.cdr.detectChanges(); }
          });
        }
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.error = 'Failed to load order details.';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  getMeasurementValue(key: string): string {
    if (!this.measurement) return '—';
    const val = (this.measurement as any)[key];
    return val != null ? `${val}"` : '—';
  }

  get itemsTotal(): number {
    return (this.order?.orderItems ?? []).reduce(
      (sum, i) => sum + (i.quantity * i.unitPrice), 0
    );
  }

  getStatusColor(status: string): string { return this.orderStatus.getColor(status); }
  getStatusLabel(status: string): string { return this.orderStatus.getLabel(status); }

  goEdit(): void {
    this.router.navigate(['/orders', this.orderId, 'edit']);
  }
}
