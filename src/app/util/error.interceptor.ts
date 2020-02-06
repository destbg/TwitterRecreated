import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpResponse,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { AlertService } from '../service/alert.service';
import { AuthService } from '../service/auth.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(
    private readonly authService: AuthService,
    private readonly router: Router,
    private readonly alertService: AlertService,
  ) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<any> {
    return next.handle(request).pipe(
      tap((t: HttpEvent<any>) => {
        if (t instanceof HttpResponse) {
          if (t.statusText === 'too many requests') {
            this.alertService.info(
              'You are sending too many requests to the server, please slow down!',
              'tmr',
            );
          }
        }
      }),
      catchError((err: HttpErrorResponse) => {
        if (err.status === 401) {
          // auto logout if 401 response returned from api
          this.authService.logout();
          this.router.navigate(['/auth/login']);
        }

        const error = err.statusText || err.error.message;
        return throwError(error);
      }),
    );
  }
}
