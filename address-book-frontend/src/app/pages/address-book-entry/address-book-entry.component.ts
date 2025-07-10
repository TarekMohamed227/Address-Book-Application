import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AddressBookService } from '../../core/services/address-book.service';
import { DepartmentService } from '../../core/services/department.service';
import { JobTitleService } from '../../core/services/job-title.service';
import { Department } from '../../core/models/department.model';
import { JobTitle } from '../../core/models/job-title.model';
import { AddressBookEntry, UpdateAddressBookEntryDto } from '../../core/models/address-book.model';
import { exportToExcel } from '../../core/utils/export-to-excel';

@Component({
  selector: 'app-address-book-entry',
  templateUrl: './address-book-entry.component.html',
  styleUrls: ['./address-book-entry.component.css']
})
export class AddressBookEntryComponent implements OnInit {
  form!: FormGroup;
  editForm!: FormGroup;
  searchForm!: FormGroup;

  departments: Department[] = [];
  jobTitles: JobTitle[] = [];
  entries: AddressBookEntry[] = [];
  allEntries: AddressBookEntry[] = [];

  selectedFile: File | null = null;
  editSelectedFile: File | null = null;

  editingId: number | null = null;
  loading = false;
  showCreateForm = false;
  showSearchForm = false;
  showModal = false;

  constructor(
    private fb: FormBuilder,
    private addressBookService: AddressBookService,
    private departmentService: DepartmentService,
    private jobTitleService: JobTitleService
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.initEditForm();
    this.initSearchForm();
    this.loadDepartments();
    this.loadJobTitles();
    this.loadEntries();

    this.searchForm.valueChanges.subscribe(values => {
      const hasSearchValue = Object.values(values).some(v => v && v !== '');
      hasSearchValue ? this.onSearch() : this.loadEntries();
    });
  }


initForm(): void {
  this.form = this.fb.group({
    fullName: ['', [Validators.required, Validators.minLength(3)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [
  Validators.required,
  Validators.minLength(6),
  Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$/)
]],
    mobileNumber: ['', [
      Validators.required,
      Validators.pattern('^[0-9]{11}$')  
    ]],
    dateOfBirth: ['', Validators.required],
    address: [''],
    departmentId: ['', Validators.required],
    jobTitleId: ['', Validators.required],
    photo: [null]
  });
}


initEditForm(): void {
  this.editForm = this.fb.group({
    fullName: ['', [Validators.required, Validators.minLength(3)]],
    email: ['', [Validators.required, Validators.email]],
     password: ['', [
  Validators.required,
  Validators.minLength(6),
  Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$/)
]],
    mobileNumber: ['', [
      Validators.required,
      Validators.pattern('^[0-9]{11}$') 
    ]],
    dateOfBirth: ['', Validators.required],
    address: [''],
    departmentId: ['', Validators.required],
    jobTitleId: ['', Validators.required]
  });
}



  initSearchForm(): void {
    this.searchForm = this.fb.group({
      fullName: [''],
      email: [''],
      mobileNumber: [''],
      departmentId: [null],
      jobTitleId: [null],
      dateOfBirthFrom: [''],
      dateOfBirthTo: ['']
    });
  }


  loadDepartments(): void {
    this.departmentService.getDepartments().subscribe({
      next: res => (this.departments = res.data),
      error: err => console.error('Failed to load departments:', err)
    });
  }

  loadJobTitles(): void {
    this.jobTitleService.getAll().subscribe({
      next: res => (this.jobTitles = res),
      error: err => console.error('Failed to load job titles:', err)
    });
  }

  loadEntries(): void {
    this.addressBookService.getAll().subscribe({
      next: res => {
        this.entries = res;
        this.allEntries = res;
      },
      error: err => console.error('Failed to load entries:', err)
    });
  }


  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0];
  }

  onSubmit(): void {
  this.form.markAllAsTouched();
  if (this.form.invalid) return;


  const { email, mobileNumber } = this.form.value;
  const isDuplicate = this.allEntries.some(entry =>
    entry.Email.toLowerCase() === email.toLowerCase() ||
    entry.MobileNumber === mobileNumber
  );

  if (isDuplicate) {
    alert('An entry with this email or mobile number already exists.');
    return;
  }

  
  const formData = new FormData();
  Object.entries(this.form.value).forEach(([key, value]) => {
    formData.append(key, value !== null ? String(value) : '');
  });

  if (this.selectedFile) {
    formData.append('photo', this.selectedFile);
  }

  this.loading = true;

  this.addressBookService.create(formData).subscribe({
    next: () => {
      alert('Entry created successfully!');
      this.form.reset();
      this.selectedFile = null;
      this.loading = false;
      this.loadEntries();
    },
    error: err => {
      console.error('Error creating entry:', err);
      this.loading = false;
    }
  });
}


 onEdit(entry: AddressBookEntry): void {
  this.editingId = entry.Id!;
  this.editSelectedFile = null;

  const dateOfBirth = entry.DateOfBirth
    ? new Date(entry.DateOfBirth).toISOString().split('T')[0]
    : '';

  this.editForm.patchValue({
    fullName: entry.FullName || '',
    email: entry.Email || '',
    password: '',
    mobileNumber: entry.MobileNumber || '',
    dateOfBirth,
    address: entry.Address || '',
    departmentId: entry.DepartmentId || '',
    jobTitleId: entry.JobTitleId || ''
  });

  this.showModal = true;
}

  optionalMinLength(controlName: string, length: number) {
  return (form: FormGroup) => {
    const control = form.get(controlName);
    if (control && control.value && control.value.length < length) {
      control.setErrors({ minLength: true });
    } else {
      if (control?.hasError('minLength')) {
        delete control.errors?.['minLength'];
        if (Object.keys(control.errors || {}).length === 0) {
          control.setErrors(null);
        }
      }
    }
  };
}


 onUpdate(): void {
  this.editForm.markAllAsTouched();
  if (!this.editingId || this.editForm.invalid) return;

  const email = this.editForm.value.email.toLowerCase();
  const mobileNumber = this.editForm.value.mobileNumber;

  
  const isDuplicate = this.allEntries.some(entry =>
    entry.Id !== this.editingId &&
    (
      entry.Email.toLowerCase() === email ||
      entry.MobileNumber === mobileNumber
    )
  );

  if (isDuplicate) {
    alert('Another entry with this email or mobile number already exists.');
    return;
  }

  const updatedData: UpdateAddressBookEntryDto = {
    id: this.editingId,
    fullName: this.editForm.value.fullName,
    email,
    password: '',
    mobileNumber,
    dateOfBirth: this.editForm.value.dateOfBirth,
    address: this.editForm.value.address,
    jobTitleId: this.editForm.value.jobTitleId,
    departmentId: this.editForm.value.departmentId
  };

  const formData = new FormData();
  Object.entries(updatedData).forEach(([key, value]) => {
    formData.append(key, value ? String(value) : '');
  });

  if (this.editSelectedFile) {
    formData.append('photo', this.editSelectedFile);
  }

  this.loading = true;

  this.addressBookService.update(this.editingId, formData).subscribe({
    next: () => {
      alert('Entry updated successfully!');
      this.closeModal();
      this.loadEntries();
      this.loading = false;
    },
    error: err => {
      console.error('Update failed:', err);
      this.loading = false;
    }
  });
}

  onDelete(id: number): void {
    if (!confirm('Are you sure you want to delete this entry?')) return;

    this.addressBookService.delete(id).subscribe({
      next: () => {
        alert('Entry deleted successfully!');
        this.loadEntries();
      },
      error: err => console.error('Delete failed:', err)
    });
  }

  onSearch(): void {
    const filters = this.searchForm.value;

    this.entries = this.allEntries.filter(entry => {
      const matchesFullName = filters.fullName ? entry.FullName?.toLowerCase().includes(filters.fullName.toLowerCase()) : true;
      const matchesEmail = filters.email ? entry.Email?.toLowerCase().includes(filters.email.toLowerCase()) : true;
      const matchesMobile = filters.mobileNumber ? entry.MobileNumber?.includes(filters.mobileNumber) : true;
      const matchesDepartment = filters.departmentId ? entry.DepartmentId === +filters.departmentId : true;
      const matchesJobTitle = filters.jobTitleId ? entry.JobTitleId === +filters.jobTitleId : true;

      const entryDate = new Date(entry.DateOfBirth);
      const fromDate = filters.dateOfBirthFrom ? new Date(filters.dateOfBirthFrom) : null;
      const toDate = filters.dateOfBirthTo ? new Date(filters.dateOfBirthTo) : null;

      const matchesDate = (!fromDate || entryDate >= fromDate) && (!toDate || entryDate <= toDate);

      return matchesFullName && matchesEmail && matchesMobile && matchesDepartment && matchesJobTitle && matchesDate;
    });
  }

  onResetSearch(): void {
    this.searchForm.reset();
    this.entries = [...this.allEntries];
  }


  toggleCreateForm(): void {
    this.showCreateForm = !this.showCreateForm;
    if (this.showCreateForm) this.showSearchForm = false;
  }

  toggleSearchForm(): void {
    this.showSearchForm = !this.showSearchForm;
    if (this.showSearchForm) this.showCreateForm = false;
  }

  closeModal(): void {
    this.showModal = false;
  }

  exportToExcel(): void {
    const fileName = 'address_book_entries';
    exportToExcel(this.entries, this.jobTitles, this.departments, fileName);
  }
}
