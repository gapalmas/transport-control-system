import { BaseEntity } from './base-entity.model';

// Place type enum matching backend
export enum PlaceType {
  Terminal = 1,
  Station = 2,
  Warehouse = 3,
  Port = 4,
  Airport = 5,
  DistributionCenter = 6,
  CustomerSite = 7,
  Other = 8
}

// Place status enum matching backend
export enum PlaceStatus {
  Active = 1,
  Inactive = 2,
  Maintenance = 3,
  Closed = 4
}

// Place interface matching backend Place entity
export interface Place extends BaseEntity {
  name: string;
  code?: string;
  description?: string;
  address?: string;
  city?: string;
  state?: string;
  country?: string;
  postalCode?: string;
  latitude?: number;
  longitude?: number;
  type: PlaceType;
  status: PlaceStatus;
  isOriginAllowed: boolean;
  isDestinationAllowed: boolean;
  contactPerson?: string;
  contactPhone?: string;
  contactEmail?: string;
  operatingHoursStart?: string; // TimeSpan as string
  operatingHoursEnd?: string;   // TimeSpan as string
  specialInstructions?: string;
}
