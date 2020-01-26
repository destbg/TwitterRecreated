import { Component, OnInit, HostListener } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { IPost } from 'src/app/model/post.model';
import { PostService } from 'src/app/service/post.service';

@Component({
  selector: 'app-multimedia-profile',
  templateUrl: './multimedia-profile.component.html',
})
export class MultimediaProfileComponent implements OnInit {
  private isProcessing: boolean;
  private hasReachedEnd: boolean;
  private username: string;
  posts: IPost[];

  constructor(
    private readonly route: ActivatedRoute,
    private readonly postService: PostService,
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.username = params.username;
      this.posts = this.route.snapshot.data.posts;
      if (this.posts.length < 50) {
        this.hasReachedEnd = true;
      }
    });

    if (typeof document != 'undefined') {
      document.body.addEventListener('touchmove', () => {
        this.onTouchScroll();
      });
    }
  }

  onTouchScroll(): void {
    const pos = document.body.scrollTop;
    this.scrolled(pos);
  }

  @HostListener('window:scroll')
  onScroll(): void {
    const pos = document.documentElement.scrollTop;
    this.scrolled(pos);
  }

  private scrolled(pos: number): void {
    if (
      !this.isProcessing &&
      !this.hasReachedEnd &&
      document.documentElement.scrollHeight -
        document.documentElement.clientHeight -
        pos <
        2000
    ) {
      this.isProcessing = true;
      this.postService
        .getUserMultimediaPosts(
          this.username,
          this.posts[this.posts.length - 1].postedOn,
        )
        .subscribe((posts: IPost[]) => {
          this.posts.push.apply(this.posts, posts);
          if (posts.length < 50) {
            this.hasReachedEnd = true;
          }
          this.isProcessing = false;
        });
    }
  }
}
