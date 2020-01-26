import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Observable } from 'rxjs';
import { IPost } from '../model/post.model';
import { BookmarkService } from '../service/bookmark.service';

@Injectable({ providedIn: 'root' })
export class BookmarkResolver implements Resolve<IPost[]> {
  constructor(private readonly bookmarkService: BookmarkService) {}

  resolve(): Observable<IPost[]> {
    return this.bookmarkService.getBookmarks();
  }
}
