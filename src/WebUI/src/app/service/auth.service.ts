import { isPlatformBrowser } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IAuth, ISelfUser } from '../model/auth.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private currentUserSubject: BehaviorSubject<ISelfUser>;
  private currentUserTokenSubject: BehaviorSubject<string>;
  public currentUser: Observable<ISelfUser>;
  public currentUserToken: Observable<string>;

  constructor(
    private readonly http: HttpClient,
    @Inject(PLATFORM_ID) private readonly platformId: object,
  ) {
    if (isPlatformBrowser(platformId)) {
      this.currentUserSubject = new BehaviorSubject<ISelfUser>(
        JSON.parse(localStorage.getItem('currentUser')),
      );
      this.currentUserTokenSubject = new BehaviorSubject<string>(
        localStorage.getItem('jwttoken'),
      );
    } else {
      this.currentUserSubject = new BehaviorSubject<ISelfUser>(undefined);
      this.currentUserTokenSubject = new BehaviorSubject<string>(undefined);
    }
    this.currentUser = this.currentUserSubject.asObservable();
    this.currentUserToken = this.currentUserTokenSubject.asObservable();
  }

  public isAuthenticated(): boolean {
    return Boolean(this.currentUserTokenSubject.value);
  }

  public get currentUserValue(): ISelfUser {
    return this.currentUserSubject.value;
  }

  public get currentUserTokenValue(): string {
    return this.currentUserTokenSubject.value;
  }

  public login(
    username: string,
    password: string,
    reCaptcha: string,
  ): Observable<ISelfUser> {
    return this.http
      .post<IAuth>(`${environment.API_URL}auth/login`, {
        username,
        password,
        reCaptcha,
      })
      .pipe(
        map((value: IAuth) => {
          // store user details and jwt token in local storage to keep user logged in between page refreshes
          if (isPlatformBrowser(this.platformId)) {
            localStorage.setItem('currentUser', JSON.stringify(value.user));
            localStorage.setItem('jwttoken', value.token);
          }
          this.currentUserTokenSubject.next(value.token);
          this.currentUserSubject.next(value.user);
          return value.user;
        }),
      );
  }

  public register(
    username: string,
    email: string,
    password: string,
    reCaptcha: string,
  ): Observable<ISelfUser> {
    return this.http
      .post<IAuth>(`${environment.API_URL}auth/register`, {
        username,
        email,
        password,
        reCaptcha,
      })
      .pipe(
        map((value: IAuth) => {
          // store user details and jwt token in local storage to keep user logged in between page refreshes
          if (isPlatformBrowser(this.platformId)) {
            localStorage.setItem('currentUser', JSON.stringify(value.user));
            localStorage.setItem('jwttoken', value.token);
          }
          this.currentUserTokenSubject.next(value.token);
          this.currentUserSubject.next(value.user);
          return value.user;
        }),
      );
  }

  public logout(): void {
    // remove user from local storage to log user out
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem('currentUser');
      localStorage.removeItem('jwttoken');
    }
    this.currentUserSubject.next(undefined);
    this.currentUserTokenSubject.next(undefined);
  }

  public changeImage(image: string): void {
    this.currentUserSubject.value.image = image;
    localStorage.setItem(
      'currentUser',
      JSON.stringify(this.currentUserSubject.value),
    );
  }

  private async getCurrentUser(): Promise<void> {
    const user = await this.http
      .get<ISelfUser>(`${environment.API_URL}auth`)
      .toPromise();
    if (user) {
      this.currentUserSubject.value.email = user.email;
      this.currentUserSubject.value.id = user.id;
      this.currentUserSubject.value.image = user.image;
      this.currentUserSubject.value.username = user.username;
    } else {
      this.currentUserSubject.next(undefined);
      this.currentUserTokenSubject.next(undefined);
    }
  }
}
