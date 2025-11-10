import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Place } from '../models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PlaceService {
  private readonly baseUrl = `${environment.apiUrl}/api/places`;

  constructor(private http: HttpClient) {}

  /**
   * Get all active places
   */
  getPlaces(): Observable<Place[]> {
    return this.http.get<Place[]>(`${this.baseUrl}/all`);
  }

  /**
   * Get place by ID
   */
  getPlaceById(id: number): Observable<Place> {
    return this.http.get<Place>(`${this.baseUrl}/${id}`);
  }

  /**
   * Get places that can be used as origins
   */
  getOriginPlaces(): Observable<Place[]> {
    return this.http.get<Place[]>(`${this.baseUrl}/origins`);
  }

  /**
   * Get places that can be used as destinations
   */
  getDestinationPlaces(): Observable<Place[]> {
    return this.http.get<Place[]>(`${this.baseUrl}/destinations`);
  }

  /**
   * Create new place
   */
  createPlace(place: Place): Observable<Place> {
    return this.http.post<Place>(this.baseUrl, place);
  }

  /**
   * Update existing place
   */
  updatePlace(id: number, place: Place): Observable<Place> {
    return this.http.put<Place>(`${this.baseUrl}/${id}`, place);
  }
}
