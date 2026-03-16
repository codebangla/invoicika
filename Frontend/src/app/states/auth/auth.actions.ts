import {ILoginRequest} from "./auth.model";

export namespace AuthActions {
  export class Loading {
    static readonly type = '[Auth] Set As Loading';
  }

  export class Working {
    static readonly type = '[Auth] Set As Working';
  }

  export class Done {
    static readonly type = '[Auth] Set As Done';
  }

  export class Initialize {
    static readonly type = '[Auth] Initialize';
  }

  export class Login {
    static readonly type = '[Auth] Login';

    constructor(public readonly request: ILoginRequest) {
    }
  }

  export class SetCurrenUser {
    static readonly type = '[Auth] Set Current User';
  }

}


