import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpService } from './http.service';
import { Observable } from 'rxjs';
import { HttpResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private router: Router, private http: HttpService) { }

  isAuthenticated(): Observable<HttpResponse<any>>{
    return this.http.get("account/profile");
  }

  login(username: string, password: string): Observable<HttpResponse<any>>{
    return this.http.post("auth/login", {username: username, password: password});
  }

  logout(): void{
    this.http.post("auth/logout", {}).subscribe(res => console.log("logged out"));
    this.router.navigateByUrl('/login');
  }
}