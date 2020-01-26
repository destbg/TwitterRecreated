import { HttpClient } from '@angular/common/http';
import { Injectable, EventEmitter } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { IUserShort } from '../model/user.model';

@Injectable({
  providedIn: 'root',
})
export class FollowService {
  public follow: EventEmitter<string>;
  public unFollow: EventEmitter<string>;

  constructor(private readonly http: HttpClient) {
    this.follow = new EventEmitter<string>();
    this.unFollow = new EventEmitter<string>();
  }

  public followUser(username: string, follow: boolean): void {
    this.http
      .post(`${environment.API_URL}follow`, {
        username,
      })
      .subscribe(() => {
        if (follow) {
          this.follow.emit(username);
        } else {
          this.unFollow.emit(username);
        }
      });
  }

  public getSuggestedUsers(): Observable<IUserShort[]> {
    return this.http.get<IUserShort[]>(
      `${environment.API_URL}follow/suggestions`,
    );
  }
}
