import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface FieldLimit {
  min: number;
  max: number;
  nameValueId: number;
}

export interface LimitsMap {
  [fieldName: string]: FieldLimit;
}

@Injectable({ providedIn: 'root' })
export class MeasurementLimitsService {
  private apiUrl = `${environment.apiUrl}/measurementlimits`;

  constructor(private http: HttpClient) {}

  getLimits(): Observable<LimitsMap> {
    return this.http.get<LimitsMap>(this.apiUrl);
  }

  setLimit(fieldName: string, min: number, max: number): Observable<any> {
    return this.http.put(`${this.apiUrl}/${fieldName}`, { min, max });
  }
}