import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../models/class/User';
import { API_URL, AUTH_CONTROLLER, RED_BUS_CONTROLLER } from '../constants/constants';
import { Login } from '../models/class/Login';
import { IAuthResponse } from '../models/interface/AuthResponse';
import { decodeJwt, getValueByKey, hasRole, JwtPayload } from '../shared/utils/jwt.utils/jwt.utils';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  http = inject(HttpClient);

  private _token = signal<string | null>(localStorage.getItem('access_token'));
  token = computed(() => this._token());


  payload = computed<JwtPayload | null>(() => {
    const t = this._token();
    return t ? decodeJwt(t) : null;
  });

  isAdmin() { return hasRole(this.payload(), 'Admin'); }
  isUser() { return hasRole(this.payload(), 'User'); }
  
 /** Read email from common claim names */
  email = computed(() => getValueByKey(this.payload(), 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'));
  userId = computed(() => getValueByKey(this.payload(), 'UserId'));

  setToken(token: string) {
    this._token.set(token);
    localStorage.setItem('access_token', token);
  }

  clearToken() {
    this._token.set(null);
    localStorage.removeItem('access_token');
  }
  register(registerObj: User): Observable<IAuthResponse> {
    return this.http.post<IAuthResponse>(`${API_URL}/${RED_BUS_CONTROLLER.AUTH}/${AUTH_CONTROLLER.REGISTER}`, registerObj, { withCredentials: true });
  }
  login(loginObj: Login): Observable<IAuthResponse> {
    return this.http.post<IAuthResponse>(`${API_URL}/${RED_BUS_CONTROLLER.AUTH}/${AUTH_CONTROLLER.LOGIN}`, loginObj, { withCredentials: true });
  }

  refresh(): Observable<IAuthResponse> {
    return this.http.post<IAuthResponse>(`${API_URL}/${RED_BUS_CONTROLLER.AUTH}/${AUTH_CONTROLLER.REFRESH}`, {}, { withCredentials: true });
  }

  logout() {
    this.http.post<IAuthResponse>(`${API_URL}/${RED_BUS_CONTROLLER.AUTH}/${AUTH_CONTROLLER.LOGOUT}?id=${this.userId()}`, {}, { withCredentials: true }).subscribe();
    this.clearToken();
  }

  isLoggedIn() { return !!this._token(); }
}
