import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {NgZorroModule} from "@modules/ng-zorro";
import {InvoiceStatusComponent} from "../shared/components";

@NgModule({
  declarations: [
    InvoiceStatusComponent
  ],
  imports: [
    CommonModule,
    NgZorroModule
  ],
  exports: [
    CommonModule,
    InvoiceStatusComponent
  ]
})
export class SharedModule { }
