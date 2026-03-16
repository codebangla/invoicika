import {AuthState} from "./auth";

export * from './auth/auth.state';

export function getStates() {
  return [AuthState];
}
