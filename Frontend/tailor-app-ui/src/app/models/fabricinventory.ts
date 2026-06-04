export interface FabricInventory {
  fabricId?: number;
  fabricCode?: string;
  name: string;
  fabricType?: string;
  color?: string;
  supplier?: string;
  quantityInMeters: number;
  quantityInItems: number;
  lowStockThresholdMeters: number;
  lowStockThresholdItems: number;
  pricePerMeter: number;
  notes?: string;
  isLowStockMeters?: boolean;
  isLowStockItems?: boolean;
  createdDate?: string;
}