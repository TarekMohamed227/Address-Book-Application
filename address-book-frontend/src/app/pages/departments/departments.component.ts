import { Component, OnInit } from '@angular/core';
import { Department, CreateDepartmentDto } from '../../core/models/department.model';
import { DepartmentService } from '../../core/services/department.service';


@Component({
  selector: 'app-departments',
  templateUrl: './departments.component.html',
  styleUrls: ['./departments.component.css']
})
export class DepartmentsComponent implements OnInit {
  departments: Department[] = [];
  newDepartment: CreateDepartmentDto = { name: '' };
  loading = false;
selectedDepartment: Department | null = null;
editName: string = '';
showEditModal = false;

  constructor(private departmentService: DepartmentService) {}

  ngOnInit(): void {
    this.loadDepartments();
  }

  loadDepartments(): void {
    this.departmentService.getDepartments().subscribe({
      next: (res) => (this.departments = res.data),
      error: (err) => console.error('Error loading departments:', err),
    });
  }

 createDepartment(): void {
  const trimmedName = this.newDepartment.name.trim();

  if (!trimmedName) {
    alert('Department name is required');
    return;
  }

 
  const isDuplicate = this.departments.some(
    dept => dept.Name.toLowerCase() === trimmedName.toLowerCase()
  );

  if (isDuplicate) {
    alert('This department already exists.');
    return;
  }

  this.loading = true;
  this.departmentService.createDepartment({ name: trimmedName }).subscribe({
    next: (res) => {
      this.departments.push(res.data);
      this.newDepartment.name = '';
      this.loading = false;
    },
    error: (err) => {
      console.error('Error creating department:', err);
      this.loading = false;
    },
  });
}


  deleteDepartment(id: number): void {
    if (!confirm('Are you sure you want to delete this department?')) {
      return;
    }

    this.loading = true;
    this.departmentService.deleteDepartment(id).subscribe({
      next: () => {
        this.departments = this.departments.filter(dept => dept.Id !== id);
        this.loading = false;
      },
      error: (err) => {
        console.error('Error deleting department:', err);
        this.loading = false;
      }
    });
  }

  editDepartment(dept: Department): void {
  this.selectedDepartment = { ...dept };
  this.editName = dept.Name;
  this.showEditModal = true;
}
  
  cancelEdit(): void {
    this.selectedDepartment = null;
    this.editName = '';
  }
  
 saveUpdate(): void {
  if (!this.selectedDepartment || !this.editName.trim()) return;

  const updatedDept: Department = {
    Id: this.selectedDepartment.Id,
    Name: this.editName.trim()
  };

  this.loading = true;

  this.departmentService.updateDepartment(updatedDept).subscribe({
    next: () => {
      this.loadDepartments(); 
      this.closeEditModal();
      this.loading = false;
    },
    error: err => {
      console.error('Update failed:', err);
      this.loading = false;
    }
  });
}
  closeEditModal(): void {
  this.showEditModal = false;
  this.selectedDepartment = null;
  this.editName = '';
}
  
  
}
