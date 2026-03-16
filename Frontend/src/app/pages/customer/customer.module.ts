import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CustomerRoutingModule } from './customer-routing.module';
import { CustomerListComponent } from './customer-list/customer-list.component';
import { CustomerAddComponent } from './customer-add/customer-add.component';
import { CustomerEditComponent } from './customer-edit/customer-edit.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgZorroModule } from '@modules';

@NgModule({
  declarations: [
    CustomerListComponent,
    CustomerAddComponent,
    CustomerEditComponent
  ],
  imports: [
    CommonModule,
    CustomerRoutingModule,
    ReactiveFormsModule,
    NgZorroModule,
    FormsModule,
]
})
export class CustomerModule { }
