import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpService } from './http.service';
import { Observable } from 'rxjs';
import { HttpResponse } from '@angular/common/http';
import { Jwt } from 'src/types/Jwt';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private router: Router, private http: HttpService) { }

  isAuthenticated(): Observable<HttpResponse<any>>{
    return this.http.get("account/profile");
  }

  login(username: string, password: string, navUrl: string): Observable<HttpResponse<any>>{
    const response = this.http.post<Jwt>("auth/login", {username: username, password: password});
    response.subscribe(data => {
      const jwtResponse: HttpResponse<Jwt> = <HttpResponse<Jwt>>data;
      this.saveToken(jwtResponse.body?.token ?? "");
      this.router.navigateByUrl(navUrl)
    });
    return response;
  }

  logout(): void {
    this.saveToken("");
    this.router.navigateByUrl("login");
  }

  private saveToken(token: string): void{
    localStorage.setItem('bearer-token', token);
    this.http.setToken(token);
  }
}