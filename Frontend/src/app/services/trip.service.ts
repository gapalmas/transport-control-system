import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Trip, TripRequest, TripStatus, TripResponse, UpdateTripStatusRequest } from '../models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TripService {
  private readonly baseUrl = `${environment.apiUrl}/api/trips`;

  constructor(private http: HttpClient) {}

  /**
   * Get all trips with pagination
   */
  getTrips(page: number = 1, pageSize: number = 10): Observable<TripResponse[]> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<TripResponse[]>(this.baseUrl, { params });
  }

  /**
   * Get trip by ID
   */
  getTripById(id: number): Observable<Trip> {
    return this.http.get<Trip>(`${this.baseUrl}/${id}`);
  }

  /**
   * Create new trip
   */
  createTrip(trip: TripRequest): Observable<TripResponse> {
    return this.http.post<TripResponse>(this.baseUrl, trip);
  }

  /**
   * Update existing trip
   */
  updateTrip(id: number, trip: TripRequest): Observable<TripResponse> {
    return this.http.put<TripResponse>(`${this.baseUrl}/${id}`, trip);
  }

  /**
   * Update trip status
   */
  updateTripStatus(id: number, statusUpdate: UpdateTripStatusRequest): Observable<TripResponse> {
    return this.http.patch<TripResponse>(`${this.baseUrl}/${id}/status`, statusUpdate);
  }

  /**
   * Delete trip
   */
  deleteTrip(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  /**
   * Get trips by status
   */
  getTripsByStatus(status: TripStatus): Observable<TripResponse[]> {
    return this.http.get<TripResponse[]>(`${this.baseUrl}/by-status/${status}`);
  }
}
