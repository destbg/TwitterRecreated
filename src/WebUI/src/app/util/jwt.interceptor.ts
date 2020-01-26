import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../service/auth.service';
import { environment } from 'src/environments/environment';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(private readonly authService: AuthService) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler,
  ): Observable<HttpEvent<any>> {
    if (request.url.startsWith(environment.API_URL)) {
      const currentUserToken = this.authService.currentUserTokenValue;
      if (currentUserToken) {
        request = request.clone({
          setHeaders: {
            Authorization: `Bearer ${currentUserToken}`,
          },
        });
      }
    }

    return next.handle(request);
  }
}
