import { HttpClient,HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, map, tap } from 'rxjs';
import { GeneralResponse, LoginResult } from '../Models/common-model';

@Injectable({
  providedIn: 'root'
})
export class AuthserviceService {
baseUrl:string = 'http://localhost:5038';

  constructor(private router:Router,private http:HttpClient) { }
  public getHeaders(): HttpHeaders {
    const token = this.getAccessToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }
  addUsername(username: string) {
    localStorage.setItem('username', username);
  }
  addAccessToken(accessToken: string) {
    localStorage.setItem('auth_token', accessToken); 
  }
  getAccessToken() {
    return localStorage.getItem('auth_token'); 
  }
  getUsername() {
    return localStorage.getItem('username');
  }
  register(credentials: { Name: string, Email: string, Password: string, confirmPassword: string }): Observable<GeneralResponse> {
    return this.http.post<any>(this.baseUrl + '/api/Accounts/Register', credentials).pipe(
      tap(response => {
        console.log('Server response:', response); 
        if (response.success) {
          console.log('Success');
        }
      }),
      map((response: any) => {
        const generalResponse: GeneralResponse = {
          success: response.flag, 
          message: response.message || ''
        };
        return generalResponse;
      })
    );
  }
  login(credentials: { Email: string, Password: string }): Observable<LoginResult> {
    return this.http.post<any>(this.baseUrl + '/api/Accounts/Login', credentials).pipe(
      tap(response => {
        if (response.flag) { 
          console.log('Success');
          this.addAccessToken(response.token || '');
          this.addUsername(credentials.Email); 
        }
      }),
      map((response: any) => {
        const loginResult: LoginResult = {
          success: response.flag,
          message: response.message || '',
          token: response.token || ''
        };
        return loginResult;
      })
    );
  }
  
  isTokenExpired(): boolean {
    const token: string = this.getAccessToken() ?? '';
    if (!token) {
      return false;
    }
    const tokenSplit: string = token.split('.')[1];
    const decodedString: string = atob(tokenSplit);
    const jsonString = JSON.parse(decodedString);
    const expiry = jsonString.exp;
    return Math.floor(new Date().getTime() / 1000) >= expiry;
  }

  isLoggedIn(): boolean {
    return !!this.getAccessToken() && !this.isTokenExpired();
  }
  logout() {
    localStorage.removeItem('username');
    localStorage.removeItem('auth_token');
    localStorage.removeItem('refreshToken');
    this.router.navigate(['/Account/login']);
  }
}
