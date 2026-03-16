import { Component, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  NonNullableFormBuilder,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { NzMessageService } from 'ng-zorro-antd/message';
import { ItemGroupService } from 'src/app/services/item-group.service';
import { ItemService } from 'src/app/services/item.service';
import { Store } from "@ngxs/store";

@Component({
  selector: 'app-group-add',
  templateUrl: './group-add.component.html',
  styleUrls: ['./group-add.component.less'],
})
export class GroupAddComponent implements OnInit {
  addGroupForm: FormGroup;
  listOfItems: any[] = [];
  loadingItems = false;

  private user_id = this.store.selectSnapshot(state => state.auth.current.userId);

  get selectedItems(): any[] {
    const selectedIds = this.addGroupForm.get('itemIds')?.value || [];
    return this.listOfItems.filter(item => selectedIds.includes(item.itemId));
  }

  constructor(
    private readonly store: Store,
    private readonly fb: NonNullableFormBuilder,
    private readonly itemGroupService: ItemGroupService,
    private readonly itemService: ItemService,
    private readonly router: Router,
    private readonly message: NzMessageService
  ) {
    this.addGroupForm = this.fb.group({
      title: new FormControl<string | null>(null, [Validators.required]),
      description: new FormControl<string | null>(null),
      itemIds: new FormControl<string[] | null>([], [Validators.required]),
    });
  }

  ngOnInit(): void {
    this.loadItems();
  }

  loadItems(): void {
    this.loadingItems = true;
    // Loading all items for selection, might need pagination if many items
    this.itemService.getItems(1, 1000, null, null, [], null).subscribe({
      next: (response) => {
        this.listOfItems = response.items;
        this.loadingItems = false;
      },
      error: () => {
        this.message.error('Error loading items');
        this.loadingItems = false;
      }
    });
  }

  submitForm(): void {
    if (this.addGroupForm.valid) {
      const newGroup = {
        ...this.addGroupForm.value,
        user_id: this.user_id,
      };

      this.itemGroupService.createItemGroup(newGroup).subscribe({
        next: () => {
          this.message.success('Item group added successfully');
          this.router.navigate(['/groups/all']);
        },
        error: (err) => {
          this.message.error('Error adding item group');
          console.error('Error adding item group:', err);
        },
      });
    }
  }

  resetForm(e: MouseEvent): void {
    e.preventDefault();
    this.addGroupForm.reset();
  }

  onBack(): void {
    this.router.navigate(['/groups/all']);
  }
}
