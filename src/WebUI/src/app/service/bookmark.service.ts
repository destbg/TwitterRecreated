import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IPost } from '../model/post.model';

@Injectable({
  providedIn: 'root',
})
export class BookmarkService {
  constructor(private readonly http: HttpClient) {}

  public getBookmarks(): Observable<IPost[]> {
    return this.http.get<IPost[]>(`${environment.API_URL}bookmark`);
  }

  public postBookmark(postId: number): void {
    this.http
      .post(`${environment.API_URL}bookmark`, {
        postId,
      })
      .subscribe((data: any) => data);
  }

  public removeBookmark(postId: number): void {
    this.http
      .delete(`${environment.API_URL}bookmark${postId}`)
      .subscribe((data: any) => data);
  }
}
