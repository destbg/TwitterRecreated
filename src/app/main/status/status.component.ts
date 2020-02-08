import {
  Component,
  ElementRef,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { MatDialog } from '@angular/material';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Data } from '@angular/router';
import { IPost } from 'src/app/model/post.model';
import { AuthService } from 'src/app/service/auth.service';
import { LikeService } from 'src/app/service/like.service';
import { OverlayService } from 'src/app/service/overlay.service';
import { SocketService } from 'src/app/service/socket.service';
import { ReplyDialogComponent } from 'src/app/shared/reply-dialog/reply-dialog.component';
import { PostStorage } from 'src/app/storage/post.storage';

@Component({
  selector: 'app-status',
  templateUrl: './status.component.html',
})
export class StatusComponent implements OnInit, OnDestroy {
  posts: IPost[];
  post: IPost;
  username: string;

  @ViewChild('heart', { static: false }) heart: ElementRef<HTMLDivElement>;

  constructor(
    private readonly postStorage: PostStorage,
    private readonly socketService: SocketService,
    private readonly route: ActivatedRoute,
    private readonly authService: AuthService,
    private readonly title: Title,
    private readonly likeService: LikeService,
    private readonly overlayService: OverlayService,
    private readonly dialog: MatDialog,
  ) {}

  ngOnInit(): void {
    this.username = this.authService.currentUserValue.username;
    this.route.data.subscribe(async (routerData: Data) => {
      this.posts = routerData.posts;
      this.post = routerData.post;
      if (!this.post) {
        return;
      }
      this.title.setTitle(`${this.post.user.username} Post | AngularTwitter`);
      this.socketService.followPosts([this.post.id]);
      await new Promise((resolve: any) => setTimeout(resolve, 1));
      if (
        (this.post.isLiked || this.post.user.username === this.username) &&
        !this.heart.nativeElement.classList.contains('post-liked')
      ) {
        this.heart.nativeElement.classList.toggle('post-liked');
      }
    });
  }

  ngOnDestroy(): void {
    if (!this.postStorage.posts || !this.post) {
      return;
    }
    if (
      !this.postStorage.posts.some(
        (storedPost: IPost) => this.post.id === storedPost.id,
      )
    ) {
      this.socketService.unFollowPosts([this.post.id]);
    }
  }

  likePost(): void {
    if (!this.post.isLiked) {
      this.heart.nativeElement.classList.toggle('is_animating');
    } else {
      this.post.isLiked = !this.post.isLiked;
    }
    this.heart.nativeElement.classList.toggle('post-liked');
    this.likeService.likePost(this.post.id);
  }

  likeAnimationEnd(): void {
    this.heart.nativeElement.classList.toggle('is_animating');
    this.post.isLiked = !this.post.isLiked;
  }

  openOptions(optionsDiv: HTMLDivElement): void {
    this.overlayService.followDiv = optionsDiv;
    this.overlayService.event.emit('showOptionsOverlay', this.post);
  }

  replyToUser(): void {
    const dialogRef = this.dialog.open(ReplyDialogComponent, {
      width: '50%',
    });
    dialogRef.componentInstance.post = this.post;
  }

  openReplyOverlay(replyDiv: HTMLDivElement): void {
    this.overlayService.followDiv = replyDiv;
    this.overlayService.event.emit('showReplyOverlay', this.post);
  }
}
