import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

const AUTH_API = 'https://localhost:5001/api/user/';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient) { }

  login(email: string, password: string): Observable<any> {
    return this.http.post(AUTH_API + 'login', {
      email,
      password,
    }, httpOptions);
  }

  register(displayName: string, email: string, password: string, password2: string): Observable<any> {
    return this.http.post(AUTH_API + 'registration', {
      displayName,
      email,
      password,
      password2
    }, httpOptions);
  }
}
