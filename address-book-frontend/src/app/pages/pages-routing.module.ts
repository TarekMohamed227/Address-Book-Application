import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  // { path: 'address-book', component: AddressBookListComponent },
  // { path: 'job-titles', component: JobTitleListComponent },
  // { path: 'departments', component: DepartmentListComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PagesRoutingModule { }
