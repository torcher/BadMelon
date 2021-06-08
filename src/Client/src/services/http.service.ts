import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';

import { HttpClient, HttpResponse, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class HttpService {
  
  private baseUrl: string;
  constructor(private http: HttpClient){
    this.baseUrl = environment.apiBaseUrl;
  }

  get<Type>(path: string): Observable<Type>{
    return this.http.get<Type>(this.baseUrl + path,{withCredentials: true});
  }

  post<Type>(path: string, content: any): Observable<HttpResponse<Type>>{
    return this.http.post<Type>(this.baseUrl + path, content, {observe: 'response', withCredentials: true});
  }

}