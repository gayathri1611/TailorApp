import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, catchError, throwError } from 'rxjs';
import { AuthResponse, LoginRequest, RegisterRequest, AppUser } from '../models/auth';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${environment.apiUrl}/auth`;

  constructor(private http: HttpClient) {}

  login(dto: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, dto).pipe(
      tap(res => {
        localStorage.setItem('token', res.token);
        localStorage.setItem('user', JSON.stringify(res));
      }),
      catchError(err => throwError(() => err))
    );
  }

  register(dto: RegisterRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, dto, { responseType: 'text' }).pipe(
      catchError(err => throwError(() => err))
    );
  }

  updateUser(id: string, dto: Partial<RegisterRequest>): Observable<any> {
    return this.http.put(`${this.apiUrl}/users/${id}`, dto, { responseType: 'text' }).pipe(
      catchError(err => throwError(() => err))
    );
  }

  getUsers(): Observable<AppUser[]> {
    return this.http.get<AppUser[]>(`${this.apiUrl}/users`).pipe(
      catchError(err => throwError(() => err))
    );
  }

  deactivateUser(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/users/${id}`).pipe(
      catchError(err => throwError(() => err))
    );
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getCurrentUser(): AuthResponse | null {
    const user = localStorage.getItem('user');
    return user ? JSON.parse(user) : null;
  }

  isLoggedIn(): boolean {
    const token = this.getToken();
    if (!token) return false;
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload.exp * 1000 > Date.now();
    } catch {
      return false;
    }
  }

  getRole(): string {
    return this.getCurrentUser()?.role ?? '';
  }

  isAdmin(): boolean {
    return this.getRole() === 'Admin';
  }

  getGoogleLoginUrl(): string {
    return `${this.apiUrl}/google`;
  }
}
