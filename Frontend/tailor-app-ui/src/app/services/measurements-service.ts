import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { Measurement } from '../models/measurement';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MeasurementService {
  private apiUrl = `${environment.apiUrl}/measurements`;

  constructor(private http: HttpClient) {}

  getMeasurements(): Observable<Measurement[]> {
    return this.http.get<Measurement[]>(this.apiUrl).pipe(
      catchError(err => throwError(() => err))
    );
  }

  getMeasurementsByCustomer(customerId: number): Observable<Measurement[]> {
    return this.http.get<Measurement[]>(`${this.apiUrl}/customer/${customerId}`).pipe(
      catchError(err => throwError(() => err))
    );
  }

  getMeasurement(id: number): Observable<Measurement> {
    return this.http.get<Measurement>(`${this.apiUrl}/${id}`).pipe(
      catchError(err => throwError(() => err))
    );
  }

  createMeasurement(measurement: Measurement): Observable<any> {
    return this.http.post(this.apiUrl, measurement).pipe(
      catchError(err => throwError(() => err))
    );
  }

  updateMeasurement(id: number, measurement: Measurement): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, measurement).pipe(
      catchError(err => throwError(() => err))
    );
  }

  deleteMeasurement(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`).pipe(
      catchError(err => throwError(() => err))
    );
  }
}
