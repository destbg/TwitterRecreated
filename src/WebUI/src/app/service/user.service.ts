import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IUser, IUserFollow, IUserShort } from '../model/user.model';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(private readonly http: HttpClient) {}

  public getUser(username: string): Observable<IUser> {
    return this.http.get<IUser>(`${environment.API_URL}user/${username}`);
  }

  public getFollowingFollowers(): Observable<IUserShort[]> {
    return this.http.get<IUserShort[]>(
      `${environment.API_URL}follow/followingFollowers`,
    );
  }

  public getUserSearch(q: string): Observable<IUserFollow[]> {
    return this.http.get<IUserFollow[]>(
      `${environment.API_URL}search/user/${q}`,
    );
  }

  public changeUserProfile(
    displayName: string,
    description: string,
    image: File,
    thumbnail: File,
  ): Observable<{ image: string; thumbnail: string }> {
    const formData = new FormData();
    if (displayName) {
      formData.append('displayName', displayName);
    }
    if (description) {
      formData.append('description', description);
    }
    if (image) {
      formData.append('image', image);
    }
    if (thumbnail) {
      formData.append('thumbnail', thumbnail);
    }
    return this.http.post<{ image: string; thumbnail: string }>(
      `${environment.API_URL}user/profile`,
      formData,
    );
  }
}
