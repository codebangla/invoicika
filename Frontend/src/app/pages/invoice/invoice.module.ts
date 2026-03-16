import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { InvoiceRoutingModule } from './invoice-routing.module';
import { InvoiceListComponent } from './invoice-list/invoice-list.component';
import { InvoiceAddComponent } from './invoice-add/invoice-add.component';
import { InvoiceEditComponent } from './invoice-edit/invoice-edit.component';
import { ReactiveFormsModule } from '@angular/forms';
import { InvoiceShowComponent } from './invoice-show/invoice-show.component';
import { NgZorroModule } from '@modules/ng-zorro/ng-zorro.module';
import {SharedModule} from "@modules";

@NgModule({
  declarations: [
    InvoiceListComponent,
    InvoiceAddComponent,
    InvoiceEditComponent,
    InvoiceShowComponent,
  ],
  imports: [
    CommonModule,
    InvoiceRoutingModule,
    ReactiveFormsModule,
    NgZorroModule,
    SharedModule
  ],
})
export class InvoiceModule {}
