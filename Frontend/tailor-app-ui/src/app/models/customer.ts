export interface Customer {
  customerId?: number;
  shopId: number;
  firstName: string;
  lastName?: string;
  phoneNumber: string;
  email?: string;
  address?: string;
}