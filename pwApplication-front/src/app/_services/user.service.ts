import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import { Observable } from 'rxjs';

const API_URL = 'https://localhost:5001/api/';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private http: HttpClient) { }

  getPublicContent(): Observable<any> {
    return this.http.get(API_URL + 'all', { responseType: 'text' });
  }

  getUserBoard(): Observable<any> {
    return this.http.get(API_URL + 'user/userInfo', { responseType: 'text' });
  }

  getUserByName(userName: string): Observable<any> {
    const params = new HttpParams()
      .set('displayName', userName);
    return this.http.get(API_URL + 'user/searchByName', { responseType: 'text', params });
  }


  getTransactions(): Observable<any> {
    return this.http.get(API_URL + 'transaction', { responseType: 'text' });
  }

  commitTransaction(receiverUserId: string, amount: number): Observable<any> {
    return this.http.post(API_URL + 'transaction', {
      receiverUserId,
      amount
    }, httpOptions);
  }
}
