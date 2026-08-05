import { Injectable } from '@angular/core';
import { AuthResponse } from '../models/auth';

@Injectable({ providedIn: 'root' })
export class StorageService {
  private readonly TOKEN_KEY = 'token';
  private readonly USER_KEY  = 'user';

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  setToken(token: string): void {
    localStorage.setItem(this.TOKEN_KEY, token);
  }

  getUser(): AuthResponse | null {
    const raw = localStorage.getItem(this.USER_KEY);
    return raw ? JSON.parse(raw) as AuthResponse : null;
  }

  setUser(data: AuthResponse): void {
    localStorage.setItem(this.USER_KEY, JSON.stringify(data));
  }

  clear(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.USER_KEY);
  }
}
