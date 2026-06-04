import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Measurement } from '../models/measurement';

@Injectable({
  providedIn: 'root'
})
export class MeasurementService {
  private apiUrl = 'http://localhost:5292/api/measurements';

  constructor(private http: HttpClient) {}

  getMeasurements(): Observable<Measurement[]> {
    return this.http.get<Measurement[]>(this.apiUrl);
  }

  getMeasurementsByCustomer(customerId: number): Observable<Measurement[]> {
    return this.http.get<Measurement[]>(`${this.apiUrl}/customer/${customerId}`);
  }

  getMeasurement(id: number): Observable<Measurement> {
    return this.http.get<Measurement>(`${this.apiUrl}/${id}`);
  }

  createMeasurement(measurement: Measurement): Observable<any> {
    return this.http.post(this.apiUrl, measurement);
  }

  updateMeasurement(id: number, measurement: Measurement): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, measurement);
  }

  deleteMeasurement(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}