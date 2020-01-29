import {
  AfterViewInit,
  Component,
  ElementRef,
  Input,
  ViewChild,
  OnInit,
  OnDestroy,
} from '@angular/core';
import { MatDialog } from '@angular/material';
import { Router } from '@angular/router';
import { IPost, IPostLike } from 'src/app/model/post.model';
import { LikeService } from 'src/app/service/like.service';
import { OverlayService } from 'src/app/service/overlay.service';
import { ReplyDialogComponent } from '../reply-dialog/reply-dialog.component';
import { Subscription } from 'rxjs';
import { IPollVoted, IPoll } from 'src/app/model/poll.model';
import { SocketService } from 'src/app/service/socket.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
  styleUrls: ['./post.component.scss'],
})
export class PostComponent implements OnInit, AfterViewInit {
  API_URL = environment.API_URL.replace('api/', '');
  @Input() post: IPost;
  @Input() displayTop: boolean;
  @Input() username: string;

  @ViewChild('heart', { static: false })
  private readonly heart: ElementRef<HTMLDivElement>;

  constructor(
    private readonly socketService: SocketService,
    private readonly router: Router,
    private readonly overlayService: OverlayService,
    private readonly likeService: LikeService,
    private readonly dialog: MatDialog,
  ) {
    this.displayTop = true;
  }

  ngOnInit(): void {
    this.socketService.hubConnection.on(
      'votedOnPoll',
      (pollVote: IPollVoted) => {
        if (this.post.id !== pollVote.postId) {
          return;
        }
        const index = this.post.poll.findIndex(
          (f: IPoll) => f.id === pollVote.optionId,
        );
        if (index !== -1) {
          this.post.poll[index].votes++;
        }
      },
    );
    this.socketService.hubConnection.on('likedPost', (postLike: IPostLike) => {
      if (this.post.id !== postLike.postId) {
        return;
      }
      if (postLike.isLike) {
        this.post.likes++;
      } else {
        this.post.likes--;
      }
    });
  }

  ngAfterViewInit(): void {
    if (!this.post) {
      return;
    }
    if (this.post.isLiked || this.post.user.username === this.username) {
      this.heart.nativeElement.classList.toggle('post-liked');
    }
  }

  navigateToPost(): void {
    this.router.navigate(['/status/' + this.post.id]);
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

  replyToUser(): void {
    const dialogRef = this.dialog.open(ReplyDialogComponent, {
      width: '50%',
    });
    dialogRef.componentInstance.post = this.post;
  }

  openOptions(optionsDiv: HTMLDivElement): void {
    this.overlayService.followDiv = optionsDiv;
    this.overlayService.event.emit('showOptionsOverlay', this.post);
  }

  openReplyOverlay(replyDiv: HTMLDivElement): void {
    this.overlayService.followDiv = replyDiv;
    this.overlayService.event.emit('showReplyOverlay', this.post);
  }

  openUserOptionsOverlay(userOptionsDiv: HTMLDivElement): void {
    this.overlayService.followDiv = userOptionsDiv;
    this.overlayService.event.emit('showUserOptionsOverlay', this.post);
  }

  joinReposters(): string {
    return this.post.reposters.join(', ');
  }

  joinLikers(): string {
    return this.post.likers.join(', ');
  }
}
