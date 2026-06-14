import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { NameValue } from '../models/namevalue';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class NameValueService {
  private apiUrl = `${environment.apiUrl}/namevalues`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<NameValue[]> {
    return this.http.get<NameValue[]>(this.apiUrl);
  }

  getByCategory(category: string): Observable<NameValue[]> {
    return this.http.get<NameValue[]>(`${this.apiUrl}/category/${category}`);
  }

  create(item: Partial<NameValue>): Observable<any> {
    return this.http.post(this.apiUrl, item);
  }

  update(id: number, item: Partial<NameValue>): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, item);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}