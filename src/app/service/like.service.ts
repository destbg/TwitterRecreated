import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class LikeService {
  constructor(private readonly http: HttpClient) {}

  public likePost(postId: number): void {
    this.http
      .post(`${environment.API_URL}like`, {
        postId,
      })
      .subscribe((s: any) => s);
  }
}
