import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';

import { HttpClient, HttpResponse, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class HttpService {
  
  private baseUrl: string = "api/";
  private bearerToken: string = "";
  constructor(private http: HttpClient){
    this.setToken(localStorage.getItem("bearer-token") ?? "");
  }

  setToken(token: string){
    this.bearerToken = "Bearer " + token;
  }

  get<Type>(path: string): Observable<Type>{
    return this.http.get<Type>(this.baseUrl + path,
      {
        headers: new HttpHeaders().set('Authorization', this.bearerToken)
      });
  }

  post<Type>(path: string, content: any): Observable<HttpResponse<Type>>{
    return this.http.post<Type>(this.baseUrl + path, content, {
      observe: 'response', 
      headers: new HttpHeaders().set('Authorization', this.bearerToken)
    });
  }

  delete(path: string): Observable<HttpResponse<any>>{
    return this.http.delete(this.baseUrl + path, {
      observe: 'response', 
      headers: new HttpHeaders().set('Authorization', this.bearerToken)
    });
  }

}