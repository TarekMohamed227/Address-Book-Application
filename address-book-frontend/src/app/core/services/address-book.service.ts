import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { AddressBookEntry } from '../models/address-book.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AddressBookService {
  
    private apiUrl = `${environment.apiUrl}/AddressBookEntry`; 

  constructor(private http: HttpClient) {}

   getAll(): Observable<AddressBookEntry[]> {
    return this.http.get<AddressBookEntry[]>(this.apiUrl).pipe(
      map(response => {
        return response.map(entry => ({
          ...entry,
          PhotoUrl: entry.PhotoUrl ? entry.PhotoUrl : ''
        }));
      })
    );
  }

  getById(id: number): Observable<AddressBookEntry> {
    return this.http.get<AddressBookEntry>(`${this.apiUrl}/${id}`);
  }

  create(payload: FormData): Observable<any> {
    return this.http.post(`${this.apiUrl}`, payload);
  }

  update(id: number, payload: FormData): Observable<void> {
  return this.http.put<void>(`${this.apiUrl}/${id}`, payload);
}

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}


