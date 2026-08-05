import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { AppUser, RegisterRequest } from '../models/auth';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class UserService {
  private apiUrl = `${environment.apiUrl}/users`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<AppUser[]> {
    return this.http.get<AppUser[]>(this.apiUrl).pipe(
      catchError(err => throwError(() => err))
    );
  }

  register(dto: RegisterRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, dto, { responseType: 'text' }).pipe(
      catchError(err => throwError(() => err))
    );
  }

  update(id: string, dto: { firstName: string; lastName: string; role: string; shopId: number }): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, dto, { responseType: 'text' }).pipe(
      catchError(err => throwError(() => err))
    );
  }

  deactivate(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`).pipe(
      catchError(err => throwError(() => err))
    );
  }
}
