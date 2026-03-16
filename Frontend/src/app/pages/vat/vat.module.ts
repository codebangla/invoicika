import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { VatRoutingModule } from './vat-routing.module';
import { VatListComponent } from './vat-list/vat-list.component';
import { VatAddComponent } from './vat-add/vat-add.component';
import { VatEditComponent } from './vat-edit/vat-edit.component';
import { ReactiveFormsModule } from '@angular/forms';
import { NgZorroModule } from '@modules';


@NgModule({
  declarations: [
    VatListComponent,
    VatAddComponent,
    VatEditComponent
  ],
  imports: [
    CommonModule,
    VatRoutingModule,
    ReactiveFormsModule,
    NgZorroModule,
  ]
})
export class VatModule { }
