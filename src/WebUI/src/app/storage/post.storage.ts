import { isPlatformServer } from '@angular/common';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import * as moment from 'moment';
import { ISelfUser } from '../model/auth.model';
import { IPost } from '../model/post.model';
import { AuthService } from '../service/auth.service';
import { FollowService } from '../service/follow.service';
import { PostService } from '../service/post.service';
import { SocketService } from '../service/socket.service';

@Injectable({
  providedIn: 'root',
})
export class PostStorage {
  private isProcessing: boolean;
  private username: string;

  private postsArray: IPost[];
  get posts(): IPost[] {
    if (this.postsArray.length === 0) {
      return undefined;
    }
    return this.postsArray;
  }
  set posts(posts: IPost[]) {
    if (posts.length < 50) {
      this.hasReachedEnd = true;
    }
    this.hasProcessed = true;
    this.socketService.followPosts(posts.map((post: IPost) => post.id));
    this.postsArray.push.apply(this.postsArray, posts);
  }

  public hasProcessed: boolean;
  public scrollPos: number;
  public hasReachedEnd: boolean;

  constructor(
    private readonly socketService: SocketService,
    private readonly postService: PostService,
    authService: AuthService,
    followService: FollowService,
    @Inject(PLATFORM_ID) platformId: object,
  ) {
    this.isProcessing = false;
    this.scrollPos = 0;
    this.hasReachedEnd = false;
    this.postsArray = new Array<IPost>();
    if (isPlatformServer(platformId)) {
      return;
    }
    authService.currentUser.subscribe((user: ISelfUser) => {
      this.handleUserChange(user);
    });
    followService.follow.subscribe(async (username: string) => {
      await this.handleFollowUser(username);
    });
    followService.unFollow.subscribe(async (username: string) => {
      await this.handleUnFollowUser(username);
    });
    socketService.hubConnection.on('deletedPost', (id: number) => {
      this.handleDeletedPost(id);
    });
    socketService.hubConnection.on('newPost', (post: IPost) => {
      this.handleNewPost(post);
    });
  }

  public scrolled(): void {
    if (
      !this.isProcessing &&
      !this.hasReachedEnd &&
      document.documentElement.scrollHeight -
        document.documentElement.clientHeight -
        this.scrollPos <
        2000
    ) {
      this.isProcessing = true;
      this.postService
        .getPosts(this.postsArray[this.postsArray.length - 1].postedOn)
        .subscribe((posts: IPost[]) => {
          this.posts = posts;
          this.isProcessing = false;
        });
    }
  }

  private handleUserChange(user: ISelfUser): void {
    this.postsArray = [];
    if (!user) {
      this.hasProcessed = true;
      return;
    }
    this.hasProcessed = false;
    this.username = user.username;
    this.postService.getPosts().subscribe((posts: IPost[]) => {
      this.posts = posts;
    });
  }

  private async handleFollowUser(username: string): Promise<void> {
    if (this.scrollPos > 2000) {
      return;
    }
    this.isProcessing = true;
    let posts = await this.postService.getUserPosts(username).toPromise();
    if (!posts || posts.length === 0) {
      this.isProcessing = false;
      return;
    }
    const lastDate =
      this.postsArray.length !== 0
        ? this.postsArray[this.postsArray.length - 1].postedOn
        : new Date(0);
    if (lastDate) {
      const date = moment(lastDate);
      if (this.postsArray.length < 50) {
        let count = this.postsArray.length;
        posts = posts.filter(
          (post: IPost) => count++ < 50 || date.isBefore(moment(post.postedOn)),
        );
      } else {
        posts = posts.filter((post: IPost) =>
          date.isBefore(moment(post.postedOn)),
        );
      }
    }
    this.posts = posts;
    this.postsArray = this.postsArray.sort(
      (a: IPost, b: IPost) =>
        new Date(b.postedOn).getTime() - new Date(a.postedOn).getTime(),
    );
    this.isProcessing = false;
    if (this.postsArray.length < 50) {
      this.hasReachedEnd = false;
    }
  }

  private async handleUnFollowUser(username: string): Promise<void> {
    if (this.scrollPos > 2000) {
      return;
    }
    this.isProcessing = true;
    const toRemove = this.postsArray.filter(
      (post: IPost) => post.user.username === username,
    );
    if (toRemove.length > 0) {
      this.socketService.unFollowPosts(toRemove.map((post: IPost) => post.id));
    }
    this.postsArray = this.postsArray.filter(
      (post: IPost) => post.user.username !== username,
    );
    if (this.postsArray.length === 0) {
      this.posts = await this.postService.getPosts().toPromise();
    } else if (this.postsArray.length < 50) {
      this.posts = await this.postService
        .getPosts(this.postsArray[this.postsArray.length - 1].postedOn)
        .toPromise();
    }
    this.isProcessing = false;
  }

  private handleDeletedPost(id: number): void {
    const index = this.postsArray.findIndex((post: IPost) => post.id === id);
    if (index !== -1) {
      const results = this.postsArray.splice(index, 1);
      this.socketService.unFollowPosts(results.map((post: IPost) => post.id));
    }
  }

  private handleNewPost(post: IPost): void {
    if (post.user.username === this.username) {
      post.isLiked = true;
    }
    this.postsArray.unshift(post);
    this.socketService.followPosts([post.id]);
  }
}
