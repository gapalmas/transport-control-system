import { Component, OnInit, ViewChild, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { MatSortModule, MatSort } from '@angular/material/sort';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSnackBarModule, MatSnackBar } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { Trip, Operator, Place, TripStatus, TripResponse, UpdateTripStatusRequest } from '../../models';
import { TripRequest } from '../../models/trip.model';
import { TripService, OperatorService, PlaceService } from '../../services';

@Component({
  selector: 'app-trip-management',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSnackBarModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './trip-management.html',
  styleUrl: './trip-management.scss',
})
export class TripManagement implements OnInit {
  private fb = inject(FormBuilder);
  private tripService = inject(TripService);
  private operatorService = inject(OperatorService);
  private placeService = inject(PlaceService);
  private snackBar = inject(MatSnackBar);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  tripForm: FormGroup;
  trips: TripResponse[] = [];
  operators: Operator[] = [];
  places: Place[] = [];
  dataSource = new MatTableDataSource<TripResponse>();
  displayedColumns: string[] = ['id', 'origin', 'destination', 'operator', 'scheduledDate', 'status', 'actions'];
  isEditing = false;
  editingTripId: number | null = null;
  isLoading = false;

  // Status options for the select
  statusOptions = [
    { value: TripStatus.Scheduled, label: 'Programado' },
    { value: TripStatus.InProgress, label: 'En Progreso' },
    { value: TripStatus.Completed, label: 'Completado' },
    { value: TripStatus.Cancelled, label: 'Cancelado' }
  ];

  constructor() {
    this.tripForm = this.fb.group({
      originId: ['', [Validators.required]],
      destinationId: ['', [Validators.required]],
      operatorId: ['', [Validators.required]],
      scheduledDate: ['', [Validators.required]],
      status: [TripStatus.Scheduled, [Validators.required]],
      notes: ['']
    });
  }

  ngOnInit(): void {
    this.loadData();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  private loadData(): void {
    this.isLoading = true;

    // Load trips
    this.tripService.getTrips(1, 100).subscribe({
      next: (trips: TripResponse[]) => {
        this.trips = trips || [];
        this.dataSource.data = this.trips;
      },
      error: (error: any) => {
        this.showMessage('Error al cargar los viajes');
        console.error('Error loading trips:', error);
      }
    });

    // Load operators
    this.operatorService.getOperators().subscribe({
      next: (operators: Operator[]) => {
        this.operators = operators || [];
      },
      error: (error: any) => {
        this.showMessage('Error al cargar los operadores');
        console.error('Error loading operators:', error);
      }
    });

    // Load places
    this.placeService.getPlaces().subscribe({
      next: (places: Place[]) => {
        this.places = places || [];
        this.isLoading = false;
      },
      error: (error: any) => {
        this.showMessage('Error al cargar los lugares');
        console.error('Error loading places:', error);
        this.isLoading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.tripForm.valid) {
      const tripData = this.tripForm.value;

      if (this.isEditing && this.editingTripId) {
        this.updateTrip(this.editingTripId, tripData);
      } else {
        this.createTrip(tripData);
      }
    }
  }

  private createTrip(tripData: any): void {
    this.isLoading = true;

    // Create proper dates from the form data
    const scheduledDate = new Date(tripData.scheduledDate);
    const startDateTime = new Date(scheduledDate);
    startDateTime.setHours(8, 0, 0, 0); // Default to 8:00 AM

    const endDateTime = new Date(scheduledDate);
    endDateTime.setHours(18, 0, 0, 0); // Default to 6:00 PM

    const tripRequest: TripRequest = {
      originId: tripData.originId,
      destinationId: tripData.destinationId,
      operatorId: tripData.operatorId,
      scheduledStartDateTime: startDateTime,
      scheduledEndDateTime: endDateTime,
      status: tripData.status,
      notes: tripData.notes || null
    };

    console.log('Creating trip with data:', tripRequest);

    this.tripService.createTrip(tripRequest).subscribe({
      next: (newTrip: TripResponse) => {
        this.trips.push(newTrip);
        this.dataSource.data = this.trips;
        this.resetForm();
        this.showMessage('Viaje creado exitosamente');
        this.isLoading = false;
      },
      error: (error: any) => {
        this.showMessage('Error al crear el viaje');
        console.error('Error creating trip:', error);
        this.isLoading = false;
      }
    });
  }

  private updateTrip(id: number, tripData: any): void {
    this.isLoading = true;

    // Create proper dates from the form data
    const scheduledDate = new Date(tripData.scheduledDate);
    const startDateTime = new Date(scheduledDate);
    startDateTime.setHours(8, 0, 0, 0); // Default to 8:00 AM

    const endDateTime = new Date(scheduledDate);
    endDateTime.setHours(18, 0, 0, 0); // Default to 6:00 PM

    const tripRequest: TripRequest = {
      id: id,
      originId: tripData.originId,
      destinationId: tripData.destinationId,
      operatorId: tripData.operatorId,
      scheduledStartDateTime: startDateTime,
      scheduledEndDateTime: endDateTime,
      status: tripData.status,
      notes: tripData.notes || null
    };

    console.log('Updating trip with data:', tripRequest);

    this.tripService.updateTrip(id, tripRequest).subscribe({
      next: (updatedTrip: any) => {
        // Recargar la lista completa para obtener los datos actualizados
        this.loadData();
        this.resetForm();
        this.showMessage('Viaje actualizado exitosamente');
        this.isLoading = false;
      },
      error: (error: any) => {
        this.showMessage('Error al actualizar el viaje');
        console.error('Error updating trip:', error);
        this.isLoading = false;
      }
    });
  }

  editTrip(trip: TripResponse): void {
    this.isEditing = true;
    this.editingTripId = trip.id;
    this.tripForm.patchValue({
      originId: trip.originId,
      destinationId: trip.destinationId,
      operatorId: trip.operatorId,
      scheduledDate: new Date(trip.scheduledStartDateTime),
      status: trip.status,
      notes: trip.notes
    });
  }

  deleteTrip(id: number): void {
    if (confirm('¿Está seguro de que desea eliminar este viaje?')) {
      this.isLoading = true;
      this.tripService.deleteTrip(id).subscribe({
        next: () => {
          this.trips = this.trips.filter(t => t.id !== id);
          this.dataSource.data = this.trips;
          this.showMessage('Viaje eliminado exitosamente');
          this.isLoading = false;
        },
        error: (error: any) => {
          this.showMessage('Error al eliminar el viaje');
          console.error('Error deleting trip:', error);
          this.isLoading = false;
        }
      });
    }
  }

  resetForm(): void {
    this.tripForm.reset();
    this.tripForm.patchValue({
      status: TripStatus.Scheduled
    });
    this.isEditing = false;
    this.editingTripId = null;
  }

  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }



  // Helper methods for displaying data from TripResponse
  getOriginName(trip: TripResponse): string {
    return trip.originName || 'N/A';
  }

  getDestinationName(trip: TripResponse): string {
    return trip.destinationName || 'N/A';
  }

  getTripOperatorName(trip: TripResponse): string {
    return trip.operatorName || 'N/A';
  }

  // Status update methods
  startTrip(trip: TripResponse): void {
    this.updateTripStatus(trip.id, TripStatus.InProgress, 'Viaje iniciado');
  }

  completeTrip(trip: TripResponse): void {
    this.updateTripStatus(trip.id, TripStatus.Completed, 'Viaje completado');
  }

  cancelTrip(trip: TripResponse): void {
    this.updateTripStatus(trip.id, TripStatus.Cancelled, 'Viaje cancelado');
  }

  private updateTripStatus(tripId: number, newStatus: TripStatus, message: string): void {
    const statusUpdate: UpdateTripStatusRequest = {
      status: newStatus,
      actualStartDateTime: newStatus === TripStatus.InProgress ? new Date() : undefined,
      actualEndDateTime: newStatus === TripStatus.Completed ? new Date() : undefined,
      notes: message
    };

    this.tripService.updateTripStatus(tripId, statusUpdate).subscribe({
      next: (updatedTrip) => {
        // Update the trip in the local data
        const index = this.trips.findIndex(t => t.id === tripId);
        if (index !== -1) {
          this.trips[index] = updatedTrip;
          this.dataSource.data = [...this.trips];
        }
        this.showMessage(message);
      },
      error: (error) => {
        console.error('Error updating trip status:', error);
        this.showMessage('Error al actualizar el estado del viaje');
      }
    });
  }

  // Check if status change is allowed
  canStartTrip(trip: TripResponse): boolean {
    return trip.status === TripStatus.Scheduled;
  }

  canCompleteTrip(trip: TripResponse): boolean {
    return trip.status === TripStatus.InProgress;
  }

  canCancelTrip(trip: TripResponse): boolean {
    return trip.status === TripStatus.Scheduled || trip.status === TripStatus.InProgress;
  }

  getStatusLabel(status: TripStatus): string {
    const statusOption = this.statusOptions.find(s => s.value === status);
    return statusOption ? statusOption.label : status.toString();
  }

  private showMessage(message: string): void {
    this.snackBar.open(message, 'Cerrar', {
      duration: 3000
    });
  }
}
