import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { FabricInventory } from '../models/fabricinventory';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FabricInventoryService {
  private apiUrl = `${environment.apiUrl}/fabricinventory`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<FabricInventory[]> {
    return this.http.get<FabricInventory[]>(this.apiUrl).pipe(
      catchError(err => throwError(() => err))
    );
  }

  getById(id: number): Observable<FabricInventory> {
    return this.http.get<FabricInventory>(`${this.apiUrl}/${id}`).pipe(
      catchError(err => throwError(() => err))
    );
  }

  create(fabric: FabricInventory): Observable<any> {
    return this.http.post(this.apiUrl, fabric).pipe(
      catchError(err => throwError(() => err))
    );
  }

  update(id: number, fabric: FabricInventory): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, fabric).pipe(
      catchError(err => throwError(() => err))
    );
  }

  adjustStock(id: number, metersAdj: number, itemsAdj: number, reason?: string): Observable<any> {
    return this.http.patch(`${this.apiUrl}/${id}/adjust-stock`, {
      metersAdjustment: metersAdj,
      itemsAdjustment: itemsAdj,
      reason
    }).pipe(
      catchError(err => throwError(() => err))
    );
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`).pipe(
      catchError(err => throwError(() => err))
    );
  }
}
