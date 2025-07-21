import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Page } from '../models/page.model';
import { User } from '../models/users.model';
import { catchError, Observable, throwError } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class UserService {
  private apiUrl = 'http://localhost:7020/api/users';

  constructor(private http: HttpClient) {}

  getUsers(options: {
    pageNumber: number;
    pageSize: number;
    }): Observable<Page<User>> {
    let params = new HttpParams()
      .set('pageNumber', options.pageNumber)
      .set('pageSize', options.pageSize);

    return this.http.get<Page<User>>(this.apiUrl, { params }).pipe(
      catchError((error) => {
      console.error('Failed to fetch all users:', error);
        return throwError(() => error); 
      })
    );
  }

  searchUsers(options: {
    pageNumber: number;
    pageSize: number;
    search?: string;
    sortBy?: string;
    }): Observable<Page<User>> {
    let params = new HttpParams()
      .set('pageNumber', options.pageNumber)
      .set('pageSize', options.pageSize);

    if (options.search) {
      params = params.set('search', options.search);
    }

    if (options.sortBy) {
      params = params.set('sortBy', options.sortBy);
    }

    return this.http.get<Page<User>>(this.apiUrl, { params }).pipe(
      catchError((error) => {
      console.error('Failed to fetch users:', error);
        return throwError(() => error); 
      })
    );
  }

  addUser(user: User): Observable<any> {
    return this.http.post(`${this.apiUrl}`, user).pipe(
      catchError((err: HttpErrorResponse) => {
        console.error(`Error adding user ${user}:`, err);
        return throwError(() => err);
      })
    );
  }

  viewUser(userId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${userId}`).pipe(
      catchError((err: HttpErrorResponse) => {
        console.error(`Error fetching user ${userId}:`, err);
        return throwError(() => err);
      })
    );
  }

  editUser(user: User): Observable<any> {
    return this.http.put(`${this.apiUrl}/${user.id}`, user).pipe(
      catchError(err => {
        console.error('Error updating user:', err);
        return throwError(() => err);
      })
    );
  }

  deleteUser(userId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${userId}`).pipe(
      catchError(err => {
        console.error('Error deleting user:', err);
        return throwError(() => err);
      })
    );
  }
}