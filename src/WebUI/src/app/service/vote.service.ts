import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class VoteService {
  constructor(private readonly http: HttpClient) {}

  public voteOnPost(postId: number, option: string): void {
    this.http
      .post(`${environment.API_URL}vote`, {
        postId,
        option,
      })
      .subscribe((s: any) => s);
  }
}
