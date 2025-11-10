import { BaseEntity } from './base-entity.model';
import { Operator } from './operator.model';
import { Place } from './place.model';

// Trip status enum matching backend
export enum TripStatus {
  Scheduled = 1,
  InProgress = 2,
  Completed = 3,
  Cancelled = 4,
  Delayed = 5
}

// Trip interface matching backend Trip entity
export interface Trip extends BaseEntity {
  originId: number;
  origin: Place;
  destinationId: number;
  destination: Place;
  scheduledStartDateTime: Date;
  scheduledEndDateTime: Date;
  operatorId: number;
  operator: Operator;
  actualStartDateTime?: Date;
  actualEndDateTime?: Date;
  status: TripStatus;
  estimatedDistance?: number;
  actualDistance?: number;
  notes?: string;
  vehicleId?: string;
}

// DTO for creating/updating trips (without navigation properties)
export interface TripRequest {
  id?: number;
  originId: number;
  destinationId: number;
  scheduledStartDateTime: Date;
  scheduledEndDateTime: Date;
  operatorId: number;
  actualStartDateTime?: Date;
  actualEndDateTime?: Date;
  status: TripStatus;
  estimatedDistance?: number;
  actualDistance?: number;
  notes?: string;
  vehicleId?: string;
}
