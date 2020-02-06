import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ITag } from '../model/tag.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class TagService {
  constructor(private readonly http: HttpClient) {}

  public getTopTags(): Observable<ITag[]> {
    return this.http.get<ITag[]>(`${environment.API_URL}tag`);
  }

  public searchTags(q: string): Observable<ITag[]> {
    return this.http.get<ITag[]>(`${environment.API_URL}search/tag/${q}`);
  }
}
