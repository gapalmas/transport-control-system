import { BaseEntity } from './base-entity.model';

// Operator status enum matching backend
export enum OperatorStatus {
  Active = 1,
  Inactive = 2,
  Suspended = 3,
  OnLeave = 4
}

// Operator interface matching backend Operator entity
export interface Operator extends BaseEntity {
  firstName: string;
  lastName: string;
  fullName: string; // Computed property from backend
  email?: string;
  phone?: string;
  employeeId?: string;
  licenseNumber?: string;
  licenseExpiryDate?: Date;
  status: OperatorStatus;
  dateOfBirth?: Date;
  hireDate?: Date;
  address?: string;
  emergencyContact?: string;
  emergencyPhone?: string;
}
