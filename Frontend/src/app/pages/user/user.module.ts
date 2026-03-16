import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserRoutingModule } from './user-routing.module';
import { UserEditComponent } from './user-edit/user-edit.component';
import { UserListComponent } from './user-list/user-list.component';
import { UserAddComponent } from './user-add/user-add.component';
import { ReactiveFormsModule } from '@angular/forms';
import { NgZorroModule } from '@modules';

@NgModule({
  declarations: [UserEditComponent, UserListComponent, UserAddComponent],
  imports: [
    CommonModule,
    UserRoutingModule,
    ReactiveFormsModule,
    NgZorroModule,
  ],
})
export class UserModule {}
