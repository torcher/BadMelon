import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

import { AuthService } from 'src/services/auth.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    constructor(private auth: AuthService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request).pipe( tap(() => {},
            (err: any) => {
                if (err instanceof HttpErrorResponse) {
                    if (err.status !== 0 && err.status !== 401) {
                        return;
                    }
                    
                    this.auth.logout();
                }
        }));
    }
}