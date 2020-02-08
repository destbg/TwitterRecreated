import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { IPoll, IPollVoted } from 'src/app/model/poll.model';
import { IPost, IPostLike } from 'src/app/model/post.model';
import { AuthService } from 'src/app/service/auth.service';
import { SocketService } from 'src/app/service/socket.service';
import { PostStorage } from 'src/app/storage/post.storage';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
})
export class PostListComponent implements OnInit, OnDestroy {
  @Input() posts: IPost[];
  @Input() displayTop: boolean;
  username: string;

  constructor(
    private readonly postStorage: PostStorage,
    private readonly socketService: SocketService,
    private readonly authService: AuthService,
  ) {
    this.displayTop = true;
  }

  ngOnInit(): void {
    if (!this.posts) {
      return;
    }
    if (this.authService.isAuthenticated()) {
      this.username = this.authService.currentUserValue.username;
    }
    this.socketService.hubConnection.on('deletedPost', (id: number) => {
      const index = this.posts.findIndex((post: IPost) => post.id === id);
      if (index !== -1) {
        const results = this.posts.splice(index, 1);
        this.socketService.unFollowPosts(results.map((post: IPost) => post.id));
      }
    });
    this.socketService.hubConnection.on('newPost', (post: IPost) => {
      if (post.user.username === this.username) {
        post.isLiked = true;
      }
      this.posts.unshift(post);
      this.socketService.followPosts([post.id]);
    });
    this.socketService.followPosts(this.posts.map((post: IPost) => post.id));
  }

  ngOnDestroy(): void {
    const results =
      this.postStorage.posts && this.posts
        ? this.posts
            .filter(
              (post: IPost) =>
                !this.postStorage.posts.some(
                  (storedPost: IPost) => post.id === storedPost.id,
                ),
            )
            .map((post: IPost) => post.id)
        : [];
    if (results.length !== 0) {
      this.socketService.unFollowPosts(results);
    }
  }
}
