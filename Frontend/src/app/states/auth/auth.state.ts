import {Action, NgxsOnInit, Selector, State, StateContext} from '@ngxs/store';
import {Injectable} from '@angular/core';
import {AuthCurrenUser, IAuthStateModel} from './auth.model';
import {AuthActions} from './auth.actions';
import {AuthService} from "@services/auth.service";
import {catchError, EMPTY, mergeMap} from "rxjs";
import {tap} from "rxjs/operators";
import {Navigate} from "@ngxs/router-plugin";
import {NzMessageService} from "ng-zorro-antd/message";


@State<IAuthStateModel>({
  name: 'auth',
  defaults: <IAuthStateModel>{
    busy: false,
    current: null,
  }
})
@Injectable()
export class AuthState implements NgxsOnInit {

  constructor(private readonly authService: AuthService,
              private readonly message: NzMessageService
  ) {
  }

  ngxsOnInit(ctx: StateContext<any>): void {
    ctx.dispatch(new AuthActions.Initialize());
  }

  @Selector()
  static IsLoading(state: IAuthStateModel): boolean {
    return state.loading;
  }

  @Selector()
  static IsWorking(state: IAuthStateModel): boolean {
    return state.busy;
  }

  @Selector()
  static getCurrent(state: IAuthStateModel): AuthCurrenUser | null {
    return state.current;
  }

  @Action(AuthActions.Done)
  onDone(ctx: StateContext<IAuthStateModel>) {
    ctx.patchState({
      loading: false,
      busy: false
    });
  }

  @Action(AuthActions.Loading)
  onLoading(ctx: StateContext<IAuthStateModel>) {
    ctx.patchState({
      loading: true
    });
  }

  @Action(AuthActions.Working)
  onWorking(ctx: StateContext<IAuthStateModel>) {
    ctx.patchState({
      busy: true
    });
  }

  @Action(AuthActions.Initialize)
  onInitialize(ctx: StateContext<IAuthStateModel>) {
    if (this.authService.isAuthenticated()) {
      ctx.dispatch(new AuthActions.SetCurrenUser());
    }
  }

  @Action(AuthActions.Login)
  onLogin(ctx: StateContext<IAuthStateModel>, action: AuthActions.Login) {
    const {userName, password} = action.request;
    return ctx.dispatch(new AuthActions.Working()).pipe(
      mergeMap(() => this.authService.login(userName, password)),
      tap((response) => {
        ctx.dispatch([
          new Navigate(['/dashboard']),
          new AuthActions.SetCurrenUser(),
          new AuthActions.Done()
        ]);
      }),
      catchError((err) => {

        if (err.status === 401) {
          // Show the error message from the backend using NzMessageService
          this.message.error(err.error.message || 'Invalid username or password.');
        } else {
          // Handle other error cases
          this.message.error('An unexpected error occurred. Please try again.');
        }
        return EMPTY;
      })
    )
  }

  @Action(AuthActions.SetCurrenUser)
  onSetCurrentUser(ctx: StateContext<IAuthStateModel>, action: AuthActions.SetCurrenUser) {
    const userInfo = this.authService.getUserInfo();
    console.log('userInfo', userInfo);
    ctx.patchState({
      current: userInfo
    });
  }


}
