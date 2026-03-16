import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-group-item-list',
  templateUrl: './group-item-list.component.html',
})
export class GroupItemListComponent {
  @Input() items: any[] = [];
}
