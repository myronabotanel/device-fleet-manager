import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Device {
  id?: string;
  name: string;
  manufacturer: string;
  type: string;
  operatingSystem: string;
  osVersion: string;
  processor: string;
  ramAmount: number;
  description: string;
  userId?: string | null;
}

@Injectable({
  providedIn: 'root'
})
export class DeviceService {
  private apiUrl = 'http://localhost:5019/api/device';

  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');
    return new HttpHeaders({ 'Authorization': `Bearer ${token}` });
  }

  getAll(): Observable<Device[]> {
    return this.http.get<Device[]>(this.apiUrl, { headers: this.getHeaders() });
  }

  getById(id: string): Observable<Device> {
    return this.http.get<Device>(`${this.apiUrl}/${id}`, { headers: this.getHeaders() });
  }

  create(device: Device): Observable<Device> {
    return this.http.post<Device>(this.apiUrl, device, { headers: this.getHeaders() });
  }

  update(id: string, device: Device): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, device, { headers: this.getHeaders() });
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`, { headers: this.getHeaders() });
  }

  assign(id: string): Observable<Device> {
    return this.http.put<Device>(`${this.apiUrl}/${id}/assign`, {}, { headers: this.getHeaders() });
  }

  unassign(id: string): Observable<Device> {
    return this.http.put<Device>(`${this.apiUrl}/${id}/unassign`, {}, { headers: this.getHeaders() });
  }
  generateDescription(device: Device): Observable<{ description: string }> {
  return this.http.post<{ description: string }>(
    'http://localhost:5019/api/ai/generate-description',
    {
      name: device.name,
      manufacturer: device.manufacturer,
      type: device.type,
      operatingSystem: device.operatingSystem,
      ramAmount: device.ramAmount,
      processor: device.processor
    },
    { headers: this.getHeaders() }
  );
}
}