export interface Measurement {
  measurementId?: number;
  measurementCode?: string;
  customerId: number;
  customerName?: string;
  shopId: number;

  // Upper body
  chest?: number;
  shoulder?: number;
  sleeveLength?: number;
  armHole?: number;
  neck?: number;

  // Lower body
  waist?: number;
  hip?: number;
  thigh?: number;
  knee?: number;
  inseamLength?: number;
  outseamLength?: number;

  // Full body
  height?: number;

  notes?: string;
  createdDate?: string;
}