export interface OrderItem {
  orderItemId?: number;
  orderId?: number;
  garmentType: string;
  description?: string;
  quantity: number;
  unitPrice: number;
  totalPrice?: number;
  fabricId?: number;
  fabricDetails?: string;
  specialInstructions?: string;
}

export interface Order {
  orderId?: number;
  orderCode?: string;
  customerId: number;
  customerName?: string;
  shopId: number;
  orderDate?: string;
  deliveryDate?: string;
  status?: string;
  totalAmount?: number;
  measurementId?: number;
  notes?: string;
  createdDate?: string;
  orderItems: OrderItem[];
}

export const ORDER_STATUSES = [
  'Pending',
  'InProgress',
  'ReadyForDelivery',
  'Delivered',
  'Cancelled'
];