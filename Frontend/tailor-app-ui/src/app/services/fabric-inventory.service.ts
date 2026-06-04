import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FabricInventory } from '../models/fabricinventory';

@Injectable({
  providedIn: 'root'
})
export class FabricInventoryService {
  private apiUrl = 'http://localhost:5292/api/fabricinventory';

  constructor(private http: HttpClient) {}

  getAll(): Observable<FabricInventory[]> {
    return this.http.get<FabricInventory[]>(this.apiUrl);
  }

  getById(id: number): Observable<FabricInventory> {
    return this.http.get<FabricInventory>(`${this.apiUrl}/${id}`);
  }

  create(fabric: FabricInventory): Observable<any> {
    return this.http.post(this.apiUrl, fabric);
  }

  update(id: number, fabric: FabricInventory): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, fabric);
  }

  adjustStock(id: number, metersAdj: number, itemsAdj: number, reason?: string): Observable<any> {
    return this.http.patch(`${this.apiUrl}/${id}/adjust-stock`, {
      metersAdjustment: metersAdj,
      itemsAdjustment: itemsAdj,
      reason
    });
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}