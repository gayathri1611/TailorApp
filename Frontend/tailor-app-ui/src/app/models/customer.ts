export interface Customer {
  customerId?: number;
  customerCode?: string;
  shopId: number;
  firstName: string;
  lastName?: string;
  phoneNumber: string;
  email?: string;
  address?: string;
}