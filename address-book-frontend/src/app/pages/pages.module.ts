import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { PagesRoutingModule } from './pages-routing.module';
import { HomeComponent } from './home/home.component';
import { DepartmentsComponent } from './departments/departments.component';
import { JobTitlesComponent } from './job-titles/job-titles.component';
import { AddressBookEntryComponent } from './address-book-entry/address-book-entry.component';

@NgModule({
  declarations: [
    HomeComponent,
    DepartmentsComponent,
    JobTitlesComponent,
    AddressBookEntryComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    PagesRoutingModule
  ],
   providers: [DatePipe] 
})
export class PagesModule { }
