import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SignupRoutingModule } from './signup-routing.module';
import { SignupComponent } from './signup.component';
import { ReactiveFormsModule } from '@angular/forms';
import { NgZorroModule } from '@modules';


@NgModule({
  declarations: [
    SignupComponent,

  ],
  imports: [
    CommonModule,
    SignupRoutingModule,
    ReactiveFormsModule,
    NgZorroModule,
  ]
})
export class SignupModule { }
