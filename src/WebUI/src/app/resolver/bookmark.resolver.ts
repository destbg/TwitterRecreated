import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Observable } from 'rxjs';
import { IBookmark } from '../model/bookmark.model';
import { BookmarkService } from '../service/bookmark.service';

@Injectable({ providedIn: 'root' })
export class BookmarkResolver implements Resolve<IBookmark[]> {
  constructor(private readonly bookmarkService: BookmarkService) {}

  resolve(): Observable<IBookmark[]> {
    return this.bookmarkService.getBookmarks();
  }
}
