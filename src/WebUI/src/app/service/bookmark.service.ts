import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IBookmark } from '../model/bookmark.model';

@Injectable({
  providedIn: 'root',
})
export class BookmarkService {
  constructor(private readonly http: HttpClient) {}

  public getBookmarks(skip?: Date): Observable<IBookmark[]> {
    return this.http.get<IBookmark[]>(`${environment.API_URL}bookmark/${skip}`);
  }

  public postBookmark(id: number): void {
    this.http
      .post(`${environment.API_URL}bookmark`, {
        id,
      })
      .subscribe((data: any) => data);
  }

  public removeBookmark(id: number): void {
    this.http
      .delete(`${environment.API_URL}bookmark/${id}`)
      .subscribe((data: any) => data);
  }
}
