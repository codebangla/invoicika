import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ItemRoutingModule } from './item-routing.module';
import { ItemListComponent } from './item-list/item-list.component';
import { ItemEditComponent } from './item-edit/item-edit.component';
import { ItemAddComponent } from './item-add/item-add.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgZorroModule } from '@modules';

@NgModule({
  declarations: [ItemListComponent, ItemEditComponent, ItemAddComponent],
  imports: [
    CommonModule,
    ItemRoutingModule,
    ReactiveFormsModule,
    NgZorroModule,
    FormsModule,
  ],
})
export class ItemModule {}
