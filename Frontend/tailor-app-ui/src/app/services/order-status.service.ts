import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class OrderStatusService {
  readonly statuses = ['', 'Pending', 'InProgress', 'ReadyForDelivery', 'Delivered', 'Cancelled'];

  private readonly colorMap: Record<string, string> = {
    'Pending':          '#F5C518',
    'InProgress':       '#3B82F6',
    'ReadyForDelivery': '#22C55E',
    'Delivered':        '#888888',
    'Cancelled':        '#EF4444'
  };

  private readonly labelMap: Record<string, string> = {
    'Pending':          'Pending',
    'InProgress':       'In Progress',
    'ReadyForDelivery': 'Ready for Delivery',
    'Delivered':        'Delivered',
    'Cancelled':        'Cancelled'
  };

  getColor(status: string | undefined): string {
    return this.colorMap[status ?? ''] ?? '#888888';
  }

  getLabel(status: string | undefined): string {
    return this.labelMap[status ?? ''] ?? status ?? '—';
  }
}
