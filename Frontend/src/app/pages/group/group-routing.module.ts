import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GroupListComponent } from './group-list/group-list.component';
import { GroupAddComponent } from './group-add/group-add.component';
import { GroupEditComponent } from './group-edit/group-edit.component';

const routes: Routes = [
  { path: 'all', component: GroupListComponent },
  { path: 'add', component: GroupAddComponent },
  { path: 'edit/:itemGroupId', component: GroupEditComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class GroupRoutingModule {}
