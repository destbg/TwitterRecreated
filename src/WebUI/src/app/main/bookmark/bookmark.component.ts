import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IPost } from 'src/app/model/post.model';

@Component({
  selector: 'app-bookmark',
  templateUrl: './bookmark.component.html',
})
export class BookmarkComponent implements OnInit {
  posts: IPost[];

  constructor(private readonly route: ActivatedRoute) {}

  ngOnInit(): void {
    this.posts = this.route.snapshot.data.posts;
  }
}
