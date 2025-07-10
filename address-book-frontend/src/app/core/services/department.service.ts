import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateDepartmentDto, Department } from '../models/department.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {

  private apiUrl = `${environment.apiUrl}/department`; 

  constructor(private http: HttpClient) { }

  getDepartments(): Observable<{ data: Department[] }> {
    return this.http.get<{ data: Department[] }>(this.apiUrl);
  }

  createDepartment(department: CreateDepartmentDto): Observable<{ data: Department }> {
    return this.http.post<{ data: Department }>(this.apiUrl, department);
  }
  updateDepartment(department: Department): Observable<{ message: string }> {
    return this.http.put<{ message: string }>(`${this.apiUrl}/${department.Id}`, department);
  }
  
  
  
  

  deleteDepartment(departmentId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${departmentId}`);
  }
}