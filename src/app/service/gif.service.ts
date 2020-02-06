import { isPlatformBrowser } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, PLATFORM_ID, QueryList } from '@angular/core';
import { Observable, Subscriber } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IGif, IGifCategory } from '../model/gif.model';

@Injectable({
  providedIn: 'root',
})
export class GifService {
  private readonly isBrowser: boolean;

  constructor(
    private readonly http: HttpClient,
    @Inject(PLATFORM_ID) platformId: any,
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  public getCategories(): Observable<QueryList<IGifCategory>> {
    if (this.isBrowser) {
      const stored = JSON.parse(localStorage.getItem('categories'));
      if (stored) {
        return new Observable<QueryList<IGifCategory>>(
          (observer: Subscriber<QueryList<IGifCategory>>) => {
            observer.next(stored);
            observer.complete();
          },
        );
      }
    }
    const result = this.http.get<QueryList<IGifCategory>>(
      `${environment.API_URL}gif`,
    );
    if (this.isBrowser && result) {
      result.subscribe((data: QueryList<IGifCategory>) =>
        localStorage.setItem('categories', JSON.stringify(data)),
      );
    }
    return result;
  }

  public getCategoryGifs(tag: string): Observable<QueryList<IGif>> {
    return this.http.post<QueryList<IGif>>(`${environment.API_URL}gif`, {
      tag,
    });
  }
}
