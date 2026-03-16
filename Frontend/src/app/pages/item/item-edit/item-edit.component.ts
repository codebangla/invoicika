import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormControl, FormGroup, NonNullableFormBuilder, Validators } from '@angular/forms';
import { ItemService } from 'src/app/services/item.service';
import { NzMessageService } from 'ng-zorro-antd/message';
import {Store} from "@ngxs/store";

@Component({
  selector: 'app-item-edit',
  templateUrl: './item-edit.component.html',
  styleUrls: ['./item-edit.component.less']
})
export class ItemEditComponent implements OnInit {
  editItemForm: FormGroup;
  itemId!: string;
  loading = true;
  private user_id = this.store.selectSnapshot(state => state.auth.current.userId);

  constructor(
    private readonly store: Store,
    private readonly fb: NonNullableFormBuilder,
    private readonly itemService: ItemService,
    private readonly router: Router,
    private readonly route: ActivatedRoute,
    private readonly message: NzMessageService
  ) {
    this.editItemForm = this.fb.group({
      name: new FormControl<string | null>(null, [Validators.required]),
      description: new FormControl<string | null>(null),
      purchasePrice: new FormControl<number | null>(null, [Validators.required]),
      salePrice: new FormControl<number | null>(null, [Validators.required]),
      quantity: new FormControl<number | null>(null, [Validators.required])
    });
  }

  ngOnInit(): void {
    this.itemId = this.route.snapshot.paramMap.get('itemId')!;
    this.itemService.getItemById(this.itemId).subscribe((item) => {
      this.editItemForm.patchValue({
        name: item.name,
        description: item.description,
        purchasePrice: item.purchasePrice,
        salePrice: item.salePrice,
        quantity: item.quantity
      });
      this.loading = false;
    }, error => {
      this.loading = false;
      this.message.error('Error loading item details');
    });
  }

  submitForm(): void {
    if (this.editItemForm.valid) {
      this.loading = true;
      const updatedItem = {
        itemId: this.itemId,
        user_id: this.user_id,
        ...this.editItemForm.value
      };

      this.itemService.updateItem(this.itemId, updatedItem).subscribe({
        next: () => {
          this.loading = false;
          this.message.success('Item updated successfully');
          this.router.navigate(['/items/all']);
        },
        error: (err) => {
          this.loading = false;
          this.message.error('Error updating item');
          console.error('Error updating item:', err);
        }
      });
    }
  }

  resetForm(e: MouseEvent): void {
    e.preventDefault();
    this.editItemForm.reset();
  }

  onBack(): void {
    this.router.navigate(['/items/all']);
  }
}
