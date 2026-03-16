import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RolesRoutingModule } from './roles-routing.module';
import { RoleListComponent } from './role-list/role-list.component';
import { NgZorroModule } from '@modules';


@NgModule({
  declarations: [
    RoleListComponent
  ],
  imports: [
    CommonModule,
    RolesRoutingModule,
    NgZorroModule,
  ]
})
export class RolesModule { }
