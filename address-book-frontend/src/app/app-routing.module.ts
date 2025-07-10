import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { AuthGuard } from './core/auth.guard'; 
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { AddressBookEntryComponent } from './pages/address-book-entry/address-book-entry.component';
import { DepartmentsComponent } from './pages/departments/departments.component';
import { JobTitlesComponent } from './pages/job-titles/job-titles.component';

const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'departments', component: DepartmentsComponent, canActivate: [AuthGuard] },
  { path: 'job-titles', component: JobTitlesComponent, canActivate: [AuthGuard] },
  { path: 'address-book', component: AddressBookEntryComponent, canActivate: [AuthGuard] },  
  {
    path: 'dashboard',
    canActivate: [AuthGuard],
    loadChildren: () =>
      import('./pages/pages.module').then(m => m.PagesModule)
  },
  { path: '**', redirectTo: 'home' }
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
