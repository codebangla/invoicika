export interface IAuthStateModel {
  loading: boolean;
  busy: boolean;
  current: AuthCurrenUser | null;
}

export interface ILoginRequest {
  userName: string;
  password: string;
}

export interface AuthCurrenUser {
  username: string | null;
  userId: string | null;
  role: string | null;
  expiration: number | null;
}
