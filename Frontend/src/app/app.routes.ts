import { Routes } from '@angular/router';
import { TripManagement } from './components/trip-management/trip-management';

export const routes: Routes = [
  { path: '', redirectTo: '/trips', pathMatch: 'full' },
  { path: 'trips', component: TripManagement },
  { path: '**', redirectTo: '/trips' }
];
