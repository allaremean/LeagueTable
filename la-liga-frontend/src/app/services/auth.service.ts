import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { User, UserDto } from '../models/interfaces';
import { BehaviorSubject, tap, catchError, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private url = `${environment.apiUrl}/Auth`;

  private currentUserSubject = new BehaviorSubject<User | null>(this.getUserFromStorage());
  public currentUser$ = this.currentUserSubject.asObservable();

  login(dto: UserDto) {
    return this.http.post(`${this.url}/login`, dto, { responseType: 'text' }).pipe(
      tap((token) => {
        const decoded = this.parseJwt(token);
        const role = decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || 'User';
        const username = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] || dto.username;
        const user: User = { id: 0, username: username!, role: role, token: token };
        localStorage.setItem('auth_token', token);
        localStorage.setItem('auth_user', JSON.stringify(user));
        this.currentUserSubject.next(user);
      })
    );
  }

  register(dto: UserDto) {
    return this.http.post<User>(`${this.url}/register`, dto);
  }

  logout() {
    localStorage.removeItem('auth_token');
    localStorage.removeItem('auth_user');
    this.currentUserSubject.next(null);
  }

  getAllUsers() {
    return this.http.get<User[]>(`${this.url}/getusers`);
  }

  promoteUser(username: string) {
    return this.http.post(`${this.url}/promote?username=${encodeURIComponent(username)}`, {}, { responseType: 'text' });
  }

  demoteUser(username: string) {
    return this.http.post(`${this.url}/demote?username=${encodeURIComponent(username)}`, {}, { responseType: 'text' });
  }

  deleteUser(username: string) {
    return this.http.delete(`${this.url}/${encodeURIComponent(username)}`, { responseType: 'text' });
  }

  getToken(): string | null {
    return localStorage.getItem('auth_token');
  }

  get currentUserValue(): User | null {
    return this.currentUserSubject.value;
  }

  private getUserFromStorage(): User | null {
    const userStr = localStorage.getItem('auth_user');
    return userStr ? JSON.parse(userStr) : null;
  }

  private parseJwt(token: string) {
    try {
      return JSON.parse(window.atob(token.split('.')[1]));
    } catch (e) {
      return null;
    }
  }
}
