import { EventEmitter, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = environment.apiUrl + 'auth';
  userInfoUpdated = new EventEmitter<void>();

  constructor(private http: HttpClient, private router: Router) {}

  login(username: string, password: string): Observable<any> {
    return this.http
      .post<any>(`${this.apiUrl}/login`, { username, password })
      .pipe(
        tap((response) => {
          if (response && response.token) {
            localStorage.setItem('token', response.token);
            this.userInfoUpdated.emit();
          }
        })
      );
  }

  logout(): void {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }

  isAuthenticated(): boolean {
    const token = localStorage.getItem('token');
    return !!token;
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getUserInfo(): {
    username: string | null;
    userId: string | null;
    role: string | null;
    expiration: number | null;
  } {
    const token = this.getToken();
    if (token) {
      try {
        const decodedToken: any = jwtDecode(token);
        return {
          username: decodedToken?.unique_name || null,
          userId: decodedToken?.nameid || null,
          role: decodedToken?.role || null,
          expiration: decodedToken?.exp || null
        };
      } catch (error) {
        console.error('Error decoding token:', error);
        return {
          username: null,
          userId: null,
          role: null,
          expiration: null
        };
      }
    }
    return {
      username: null,
      userId: null,
      role: null,
      expiration: null
    };
  }

  private refreshUserInfo(): void {
    const userInfo = this.getUserInfo();
  }
}
