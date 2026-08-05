import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, catchError, throwError } from 'rxjs';
import { AuthResponse, LoginRequest } from '../models/auth';
import { StorageService } from './storage.service';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${environment.apiUrl}/auth`;

  constructor(private http: HttpClient, private storage: StorageService) {}

  login(dto: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, dto).pipe(
      tap(res => this.storeAuthData(res)),
      catchError(err => throwError(() => err))
    );
  }

  storeAuthData(data: AuthResponse): void {
    this.storage.setToken(data.token);
    this.storage.setUser(data);
  }

  logout(): void {
    this.storage.clear();
  }

  getToken(): string | null {
    return this.storage.getToken();
  }

  getCurrentUser(): AuthResponse | null {
    return this.storage.getUser();
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
