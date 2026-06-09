import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { Order } from '../models/order';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private apiUrl = `${environment.apiUrl}/orders`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Order[]> {
    return this.http.get<Order[]>(this.apiUrl).pipe(
      catchError(err => throwError(() => err))
    );
  }

  getByCustomer(customerId: number): Observable<Order[]> {
    return this.http.get<Order[]>(`${this.apiUrl}/customer/${customerId}`).pipe(
      catchError(err => throwError(() => err))
    );
  }

  getById(id: number): Observable<Order> {
    return this.http.get<Order>(`${this.apiUrl}/${id}`).pipe(
      catchError(err => throwError(() => err))
    );
  }

  create(order: Order): Observable<any> {
    return this.http.post(this.apiUrl, order).pipe(
      catchError(err => throwError(() => err))
    );
  }

  update(id: number, order: Order): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, order).pipe(
      catchError(err => throwError(() => err))
    );
  }

  updateStatus(id: number, status: string): Observable<any> {
    return this.http.patch(`${this.apiUrl}/${id}/status`, { status }).pipe(
      catchError(err => throwError(() => err))
    );
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`).pipe(
      catchError(err => throwError(() => err))
    );
  }
}
