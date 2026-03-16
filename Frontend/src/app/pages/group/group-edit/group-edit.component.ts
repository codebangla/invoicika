import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormControl, FormGroup, NonNullableFormBuilder, Validators } from '@angular/forms';
import { ItemGroupService } from 'src/app/services/item-group.service';
import { ItemService } from 'src/app/services/item.service';
import { NzMessageService } from 'ng-zorro-antd/message';
import { Store } from "@ngxs/store";

@Component({
  selector: 'app-group-edit',
  templateUrl: './group-edit.component.html',
  styleUrls: ['./group-edit.component.less']
})
export class GroupEditComponent implements OnInit {
  editGroupForm: FormGroup;
  itemGroupId!: string;
  loading = true;
  listOfItems: any[] = [];
  loadingItems = false;
  private user_id = this.store.selectSnapshot(state => state.auth.current.userId);

  constructor(
    private readonly store: Store,
    private readonly fb: NonNullableFormBuilder,
    private readonly itemGroupService: ItemGroupService,
    private readonly itemService: ItemService,
    private readonly router: Router,
    private readonly route: ActivatedRoute,
    private readonly message: NzMessageService
  ) {
    this.editGroupForm = this.fb.group({
      title: new FormControl<string | null>(null, [Validators.required]),
      description: new FormControl<string | null>(null),
      itemIds: new FormControl<string[] | null>([], [Validators.required]),
    });
  }

  ngOnInit(): void {
    this.itemGroupId = this.route.snapshot.paramMap.get('itemGroupId')!;
    this.loadItems();
    this.loadGroupDetails();
  }

  loadItems(): void {
    this.loadingItems = true;
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

  loadGroupDetails(): void {
    this.itemGroupService.getItemGroupById(this.itemGroupId).subscribe((group) => {
      this.editGroupForm.patchValue({
        title: group.title,
        description: group.description,
        itemIds: group.items ? group.items.map((i: any) => i.itemId) : []
      });
      this.loading = false;
    }, error => {
      this.loading = false;
      this.message.error('Error loading item group details');
    });
  }

  submitForm(): void {
    if (this.editGroupForm.valid) {
      this.loading = true;
      const updatedGroup = {
        itemGroupId: this.itemGroupId,
        user_id: this.user_id,
        ...this.editGroupForm.value
      };

      this.itemGroupService.updateItemGroup(this.itemGroupId, updatedGroup).subscribe({
        next: () => {
          this.loading = false;
          this.message.success('Item group updated successfully');
          this.router.navigate(['/groups/all']);
        },
        error: (err) => {
          this.loading = false;
          this.message.error('Error updating item group');
          console.error('Error updating item group:', err);
        }
      });
    }
  }

  resetForm(e: MouseEvent): void {
    e.preventDefault();
    this.editGroupForm.reset();
  }

  onBack(): void {
    this.router.navigate(['/groups/all']);
  }
}
