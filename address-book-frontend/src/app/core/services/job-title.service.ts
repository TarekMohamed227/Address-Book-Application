import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateJobTitleDto, JobTitle } from '../models/job-title.model';
import { map } from 'rxjs/operators';
import { environment } from '../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class JobTitleService {
    private apiUrl = `${environment.apiUrl}/Jobtitle`; 
 
  constructor(private http: HttpClient) {}

 getAll(): Observable<JobTitle[]> {
  return this.http.get<any[]>(this.apiUrl).pipe(
    map(response =>
      response.map(item => ({
        id: item.Id,
        name: item.Name,
        addressBookEntries: item.AddressBookEntries
      }))
    )
  );
}

  

  getById(id: number): Observable<JobTitle> {
    return this.http.get<JobTitle>(`${this.apiUrl}/${id}`);
  }

  create(job: CreateJobTitleDto): Observable<{ data: JobTitle }> {
    return this.http.post<{ data: JobTitle }>(this.apiUrl, job);
  }
  

  update(id: number, dto: { id: number; name: string }): Observable<{ message: string }> {
    return this.http.put<{ message: string }>(`${this.apiUrl}/${id}`, dto);
  }
  
  
  
  
  
  

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
