import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class VoteService {
  constructor(private readonly http: HttpClient) {}

  public voteOnPost(optionId: number): void {
    this.http
      .post(`${environment.API_URL}vote`, {
        optionId,
      })
      .subscribe((s: any) => s);
  }
}
