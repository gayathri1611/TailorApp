export interface NameValue {
  nameValueId: number;
  category: string;
  value: string;
  label: string;
  sortOrder: number;
}

export const NV_CATEGORIES = {
  UNIT:         'Unit',
  GARMENT_TYPE: 'GarmentType',
  FABRIC_TYPE:  'FabricType'
};