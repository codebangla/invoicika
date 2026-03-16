import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GroupRoutingModule } from './group-routing.module';
import { GroupListComponent } from './group-list/group-list.component';
import { GroupEditComponent } from './group-edit/group-edit.component';
import { GroupAddComponent } from './group-add/group-add.component';
import { GroupItemListComponent } from '../group/components';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgZorroModule } from '@modules';

@NgModule({
  declarations: [
    GroupListComponent,
    GroupEditComponent,
    GroupAddComponent,
    GroupItemListComponent,
  ],
  imports: [
    CommonModule,
    GroupRoutingModule,
    ReactiveFormsModule,
    NgZorroModule,
    FormsModule,
  ],
})
export class GroupModule {}
