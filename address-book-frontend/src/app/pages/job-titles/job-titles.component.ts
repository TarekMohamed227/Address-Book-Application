import { Component, OnInit } from '@angular/core';
import { CreateJobTitleDto, JobTitle } from '../../core/models/job-title.model';
import { JobTitleService } from '../../core/services/job-title.service';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-job-titles',
  templateUrl: './job-titles.component.html',
  styleUrls: ['./job-titles.component.css']
})
export class JobTitlesComponent implements OnInit {
  jobTitles: JobTitle[] = [];
  newJobTitle: CreateJobTitleDto = { name: '' };
  loading = false;
  selectedJobTitle: JobTitle | null = null;
  editName: string = '';
showEditModal = false;


  constructor(private jobTitleService: JobTitleService, private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.loadJobTitles();
  }

  loadJobTitles(): void {
    this.jobTitleService.getAll().subscribe({
      next: (titles) => {
        this.jobTitles = titles;
      },
      error: (err) => {
        console.error('Failed to load job titles:', err);
      }
    });
  }

createJobTitle(): void {
  const trimmedName = this.newJobTitle.name.trim();

  if (!trimmedName) {
    alert('Job title name is required');
    return;
  }

 
  const isDuplicate = this.jobTitles.some(
    jt => jt.name.toLowerCase() === trimmedName.toLowerCase()
  );

  if (isDuplicate) {
    alert('This job title already exists.');
    return;
  }

  this.loading = true;
  this.jobTitleService.create({ name: trimmedName }).subscribe({
    next: (res) => {
      this.loadJobTitles();
      this.newJobTitle.name = '';
      this.loading = false;
    },
    error: (err) => {
      console.error('Error creating job title:', err);
      this.loading = false;
    }
  });
}


  deleteJobTitle(id: number): void {
    if (!confirm('Are you sure you want to delete this job title?')) {
      return;
    }

    this.loading = true;
    this.jobTitleService.delete(id).subscribe({
      next: () => {
        this.loadJobTitles(); 
        this.loading = false;
        this.cdr.detectChanges(); 
      },
      error: (err) => {
        console.error('Error deleting job title:', err);
        this.loading = false;
      }
    });
  }

  editJobTitle(job: JobTitle): void {
    this.selectedJobTitle = { ...job };
    this.editName = job.name;
  }

  cancelEdit(): void {
    this.selectedJobTitle = null;
    this.editName = '';
  }

 openEditModal(job: JobTitle): void {
  this.selectedJobTitle = { ...job }; 
  this.editName = job.name;
  this.showEditModal = true;
}

closeEditModal(): void {
  this.showEditModal = false;
  this.selectedJobTitle = null;
  this.editName = '';
}

saveUpdate(): void {
  if (!this.selectedJobTitle || !this.editName.trim()) {
    alert('Job title name is required');
    return;
  }

  this.loading = true;

  const updateDto = {
    id: this.selectedJobTitle.id,
    name: this.editName.trim()
  };

  this.jobTitleService.update(this.selectedJobTitle.id, updateDto).subscribe({
    next: () => {
      const idx = this.jobTitles.findIndex(j => j.id === this.selectedJobTitle!.id);
      if (idx !== -1) {
        this.jobTitles[idx].name = this.editName;
      }
      this.closeEditModal();
      this.loading = false;
    },
    error: (err) => {
      console.error('Error updating job title:', err);
      this.loading = false;
    }
  });
}
}
