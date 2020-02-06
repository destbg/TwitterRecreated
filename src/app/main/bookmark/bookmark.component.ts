import { Component, HostListener, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IBookmark } from 'src/app/model/bookmark.model';
import { BookmarkService } from 'src/app/service/bookmark.service';
import { IPost } from 'src/app/model/post.model';

@Component({
  selector: 'app-bookmark',
  templateUrl: './bookmark.component.html',
})
export class BookmarkComponent implements OnInit {
  bookmarks: IBookmark[];
  posts: IPost[];
  private isProcessing: boolean;
  private hasReachedEnd: boolean;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly bookmarkService: BookmarkService,
  ) {}

  ngOnInit(): void {
    this.bookmarks = this.route.snapshot.data.bookmarks;
    this.posts = this.bookmarks.map(f => {
      f.post.bookmarkId = f.id;
      return f.post;
    });
    if (this.bookmarks.length < 50) {
      this.hasReachedEnd = true;
    }
  }

  @HostListener('window:scroll')
  onScroll(): void {
    const pos = document.documentElement.scrollTop;
    if (
      !this.isProcessing &&
      !this.hasReachedEnd &&
      document.documentElement.scrollHeight -
        document.documentElement.clientHeight -
        pos <
        2000
    ) {
      this.isProcessing = true;
      this.bookmarkService
        .getBookmarks(this.bookmarks[this.bookmarks.length - 1].createdOn)
        .subscribe((bookmarks: IBookmark[]) => {
          if (bookmarks.length < 50) {
            this.hasReachedEnd = true;
          }
          this.bookmarks.push.apply(this.bookmarks, bookmarks);
          this.posts.push.apply(
            this.posts,
            bookmarks.map(f => {
              f.post.bookmarkId = f.id;
              return f.post;
            }),
          );
          this.isProcessing = false;
        });
    }
  }
}
