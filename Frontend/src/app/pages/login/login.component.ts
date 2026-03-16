import {Component} from '@angular/core';
import {Router} from '@angular/router';
import {Store} from '@ngxs/store'
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {AuthService} from 'src/app/services/auth.service';
import {NzMessageService} from 'ng-zorro-antd/message';
import {AuthActions} from "@states/auth";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  validateForm: FormGroup = new FormGroup({
    userName: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required]),
  });

  constructor(
    private readonly store: Store,
  ) {
  }

  submitForm(): void {
    if (this.validateForm.valid) {
      const {userName, password} = this.validateForm.value;
      this.store.dispatch(new AuthActions.Login({userName, password}));
    } else {
      Object.values(this.validateForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({onlySelf: true});
        }
      });
    }
  }
}
