import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Operator } from '../models';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OperatorService {
  private readonly baseUrl = `${environment.apiUrl}/api/operators`;

  constructor(private http: HttpClient) {}

  /**
   * Get all active operators
   */
  getOperators(): Observable<Operator[]> {
    return this.http.get<Operator[]>(this.baseUrl);
  }

  /**
   * Get operator by ID
   */
  getOperatorById(id: number): Observable<Operator> {
    return this.http.get<Operator>(`${this.baseUrl}/${id}`);
  }

  /**
   * Create new operator
   */
  createOperator(operator: Operator): Observable<Operator> {
    return this.http.post<Operator>(this.baseUrl, operator);
  }

  /**
   * Update existing operator
   */
  updateOperator(id: number, operator: Operator): Observable<Operator> {
    return this.http.put<Operator>(`${this.baseUrl}/${id}`, operator);
  }
}
