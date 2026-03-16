import {Component} from '@angular/core';
import {
  FormControl,
  FormGroup,
  NonNullableFormBuilder,
  Validators,
} from '@angular/forms';
import {Router} from '@angular/router';
import {NzMessageService} from 'ng-zorro-antd/message';
import {ItemService} from 'src/app/services/item.service';
import {Store} from "@ngxs/store";

@Component({
  selector: 'app-item-add',
  templateUrl: './item-add.component.html',
  styleUrls: ['./item-add.component.less'],
})
export class ItemAddComponent {
  addItemForm: FormGroup;

  private user_id = this.store.selectSnapshot(state => state.auth.current.userId);

  constructor(
    private readonly store: Store,
    private readonly fb: NonNullableFormBuilder,
    private readonly itemService: ItemService,
    private readonly router: Router,
    private readonly message: NzMessageService
  ) {
    this.addItemForm = this.fb.group({
      name: new FormControl<string | null>(null, [Validators.required]),
      description: new FormControl<string | null>(null),
      purchasePrice: new FormControl<number | null>(null, [
        Validators.required,
      ]),
      salePrice: new FormControl<number | null>(null, [Validators.required]),
      quantity: new FormControl<number | null>(null, [Validators.required]),
    });
  }

  submitForm(): void {
    if (this.addItemForm.valid) {
      const newItem = {
        // Generate a new GUID for itemId
        ...this.addItemForm.value,
        user_id: this.user_id,
      };

      this.itemService.createItem(newItem).subscribe({
        next: () => {
          this.message.success('Item added successfully');
          this.router.navigate(['/items/all']); // Redirect after success
        },
        error: (err) => {
          this.message.error('Error adding item');
          console.error('Error adding item:', err);
        },
      });
    }
  }

  resetForm(e: MouseEvent): void {
    e.preventDefault();
    this.addItemForm.reset();
  }

  onBack(): void {
    this.router.navigate(['/items/all']);
  }
}
