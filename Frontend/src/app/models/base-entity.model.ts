// Base entity interface matching the backend BaseEntity
export interface BaseEntity {
  id: number;
  createdAt: Date;
  modifiedAt: Date;
  createdBy?: string;
  modifiedBy?: string;
}