import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { OrderService } from '../../../services/order.service';
import { OrderStatusService } from '../../../services/order-status.service';
import { Order } from '../../../models/order';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.css']
})
export class Dashboard implements OnInit, OnDestroy {
  allOrders: Order[] = [];
  loading = false;
  today = new Date();

  toast = { message: '', visible: false };
  confirmingId: number | null = null;
  confirmingStatus: string | null = null;
  updatingId: number | null = null;

  private toastTimer: ReturnType<typeof setTimeout> | null = null;

  readonly STATUS_ORDER = ['Pending', 'InProgress', 'ReadyForDelivery', 'Delivered'];

  readonly STEPS = [
    { key: 'Pending',          short: 'Recv', label: 'Received'  },
    { key: 'InProgress',       short: 'Sewn', label: 'Stitching' },
    { key: 'ReadyForDelivery', short: 'Rdy',  label: 'Ready'     },
    { key: 'Delivered',        short: 'Done', label: 'Delivered' },
  ];

  private readonly NEXT_ACTION: Record<string, { status: string; label: string }> = {
    'Pending':          { status: 'InProgress',       label: 'Mark Stitching' },
    'InProgress':       { status: 'ReadyForDelivery', label: 'Mark Ready'     },
    'ReadyForDelivery': { status: 'Delivered',        label: 'Mark Delivered' },
  };

  private readonly DISPLAY_LABEL: Record<string, string> = {
    'Pending':          'Received',
    'InProgress':       'Stitching',
    'ReadyForDelivery': 'Ready',
    'Delivered':        'Delivered',
  };

  constructor(
    private orderService: OrderService,
    private orderStatus: OrderStatusService,
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

  ngOnDestroy(): void {
    if (this.toastTimer) clearTimeout(this.toastTimer);
  }

  // ── Board data ───────────────────────────────────────────────

  get newOrders(): Order[] {
    const todayStr = new Date().toDateString();
    return this.allOrders.filter(o =>
      o.createdDate && new Date(o.createdDate).toDateString() === todayStr
    );
  }

  get ordersDueToday(): Order[] {
    const todayStr = new Date().toDateString();
    return this.allOrders.filter(o =>
      o.deliveryDate &&
      new Date(o.deliveryDate).toDateString() === todayStr &&
      o.status !== 'Delivered' &&
      o.status !== 'Cancelled'
    );
  }

  get pendingDeliveries(): Order[] {
    return this.allOrders.filter(o => o.status === 'ReadyForDelivery');
  }

  // ── Status / date helpers ────────────────────────────────────

  getStatusColor(status: string): string { return this.orderStatus.getColor(status); }
  getStatusLabel(status: string): string { return this.orderStatus.getLabel(status); }

  isDueToday(o: Order): boolean {
    if (!o.deliveryDate) return false;
    return new Date(o.deliveryDate).toDateString() === new Date().toDateString();
  }

  isOverdue(o: Order): boolean {
    if (!o.deliveryDate || o.status === 'Delivered' || o.status === 'Cancelled') return false;
    const due = new Date(o.deliveryDate);
    due.setHours(23, 59, 59, 999);
    return due < new Date();
  }

  // ── Progress bar helpers ─────────────────────────────────────

  isDone(status: string, stepIndex: number): boolean {
    return this.STATUS_ORDER.indexOf(status) > stepIndex;
  }

  isCurr(status: string, stepIndex: number): boolean {
    return this.STATUS_ORDER.indexOf(status) === stepIndex;
  }

  progressWidth(status: string): string {
    const idx = this.STATUS_ORDER.indexOf(status);
    if (idx <= 0) return '0%';
    return `${Math.round((idx / (this.STATUS_ORDER.length - 1)) * 100)}%`;
  }

  nextAction(order: Order): { status: string; label: string } | null {
    return this.NEXT_ACTION[order.status ?? ''] ?? null;
  }

  // ── Step tap ─────────────────────────────────────────────────

  onStepTap(event: Event, order: Order, stepKey: string): void {
    event.stopPropagation();
    if (order.status === stepKey || order.status === 'Cancelled') return;
    this.requestAction(event, order, stepKey);
  }

  // ── Confirm flow ─────────────────────────────────────────────

  requestAction(event: Event, order: Order, newStatus: string): void {
    event.stopPropagation();
    this.confirmingId = order.orderId!;
    this.confirmingStatus = newStatus;
  }

  cancelConfirm(event: Event): void {
    event.stopPropagation();
    this.confirmingId = null;
    this.confirmingStatus = null;
  }

  confirmAction(event: Event, order: Order): void {
    event.stopPropagation();
    const newStatus = this.confirmingStatus;
    if (!newStatus) return;
    this.confirmingId = null;
    this.confirmingStatus = null;
    this.updatingId = order.orderId!;

    this.orderService.updateStatus(order.orderId!, newStatus).subscribe({
      next: () => {
        order.status = newStatus;
        this.updatingId = null;
        const label = this.DISPLAY_LABEL[newStatus] ?? newStatus;
        this.showToast(`${order.orderCode} marked as ${label}`);
        this.cdr.detectChanges();
      },
      error: () => {
        this.updatingId = null;
        this.showToast('Failed to update status. Try again.');
        this.cdr.detectChanges();
      }
    });
  }

  // ── Toast ─────────────────────────────────────────────────────

  showToast(message: string): void {
    if (this.toastTimer) clearTimeout(this.toastTimer);
    this.toast = { message, visible: true };
    this.cdr.detectChanges();
    this.toastTimer = setTimeout(() => {
      this.toast = { ...this.toast, visible: false };
      this.cdr.detectChanges();
    }, 2500);
  }
}
