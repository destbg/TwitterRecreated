import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class RepostService {
  constructor(private readonly http: HttpClient) {}

  public sendRepost(id: number, content?: string): void {
    this.http
      .post(`${environment.API_URL}repost`, {
        id,
        content,
      })
      .subscribe(
        (data: any) => data,
        (error: HttpErrorResponse) => console.error(error),
      );
  }
}
