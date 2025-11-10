import { TripStatus } from './trip.model';

// DTO for updating trip status
export interface UpdateTripStatusRequest {
  status: TripStatus;
  actualStartDateTime?: Date;
  actualEndDateTime?: Date;
  notes?: string;
}

// Response DTO that matches the backend TripResponseDto
export interface TripResponse {
  id: number;
  originId: number;
  originName: string;
  destinationId: number;
  destinationName: string;
  operatorId: number;
  operatorName: string;
  scheduledStartDateTime: Date;
  scheduledEndDateTime: Date;
  actualStartDateTime?: Date;
  actualEndDateTime?: Date;
  status: TripStatus;
  estimatedDistance?: number;
  actualDistance?: number;
  notes?: string;
  vehicleId?: string;
  createdAt: Date;
  modifiedAt: Date;
}

// Response DTO that matches the backend OperatorResponseDto
export interface OperatorResponse {
  id: number;
  firstName: string;
  lastName: string;
  fullName: string;
  email?: string;
  phone?: string;
  employeeId?: string;
  licenseNumber?: string;
  licenseExpiryDate?: Date;
  status: number;
  dateOfBirth?: Date;
  hireDate?: Date;
  address?: string;
  emergencyContact?: string;
  emergencyPhone?: string;
  createdAt: Date;
  modifiedAt: Date;
}

// Response DTO that matches the backend PlaceResponseDto
export interface PlaceResponse {
  id: number;
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
  type: number;
  status: number;
  isOriginAllowed: boolean;
  isDestinationAllowed: boolean;
  contactPerson?: string;
  contactPhone?: string;
  contactEmail?: string;
  operatingHoursStart?: string;
  operatingHoursEnd?: string;
  specialInstructions?: string;
  createdAt: Date;
  modifiedAt: Date;
}
