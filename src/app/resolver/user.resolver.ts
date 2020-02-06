import { isPlatformBrowser } from '@angular/common';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve } from '@angular/router';
import { Observable, of } from 'rxjs';
import { IUser, IUserFollow } from '../model/user.model';
import { UserService } from '../service/user.service';

@Injectable({ providedIn: 'root' })
export class UserResolver implements Resolve<IUser> {
  constructor(
    private readonly userService: UserService,
    @Inject(PLATFORM_ID) private readonly platformId: object,
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<IUser> {
    if (isPlatformBrowser(this.platformId)) {
      return this.userService.getUser(route.paramMap.get('username'));
    }
    return of(undefined);
  }
}

@Injectable({ providedIn: 'root' })
export class UserSearchResolver implements Resolve<IUserFollow[]> {
  constructor(
    private readonly userService: UserService,
    @Inject(PLATFORM_ID) private readonly platformId: object,
  ) {}

  resolve(): Observable<IUserFollow[]> {
    if (isPlatformBrowser(this.platformId)) {
      return this.userService.getUserSearch(
        window.location.pathname.split('/')[2],
      );
    }
    return of([]);
  }
}
