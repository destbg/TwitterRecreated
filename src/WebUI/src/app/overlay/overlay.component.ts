import {
  Component,
  ElementRef,
  HostListener,
  OnInit,
  ViewChild,
} from '@angular/core';
import { MatDialog } from '@angular/material';
import { ClipboardService } from 'ngx-clipboard';
import { OverlayService } from 'src/app/service/overlay.service';
import { IPost } from '../model/post.model';
import { AuthService } from '../service/auth.service';
import { BookmarkService } from '../service/bookmark.service';
import { PostService } from '../service/post.service';
import { RepostService } from '../service/repost.service';
import { PostFormComponent } from '../shared/post-form/post-form.component';
import { Router } from '@angular/router';
import { ISelfUser } from '../model/auth.model';

@Component({
  selector: 'app-overlay',
  templateUrl: './overlay.component.html',
})
export class OverlayComponent implements OnInit {
  optionsOverlay: boolean;
  userOptionsOverlay: boolean;
  replyOverlay: boolean;
  post: IPost;
  username: string;

  @ViewChild('div', { static: false })
  private readonly overlayDiv: ElementRef<HTMLDivElement>;
  @ViewChild('list', { static: false })
  private readonly listDiv: ElementRef<HTMLDivElement>;

  constructor(
    private readonly overlayService: OverlayService,
    private readonly authService: AuthService,
    private readonly postService: PostService,
    private readonly repostService: RepostService,
    private readonly clipboard: ClipboardService,
    private readonly bookmarkService: BookmarkService,
    private readonly dialog: MatDialog,
    public router: Router,
  ) {}

  ngOnInit(): void {
    if (!this.authService.isAuthenticated()) {
      return;
    }
    this.authService.currentUser.subscribe((selfUser: ISelfUser) => {
      if (selfUser) {
        this.username = selfUser.username;
      }
    });
    this.overlayService.event.on('showOptionsOverlay', async (post: IPost) => {
      this.optionsOverlay = true;
      await this.setPost(post);
    });
    this.overlayService.event.on('showReplyOverlay', async (post: IPost) => {
      this.replyOverlay = true;
      await this.setPost(post);
    });
    this.overlayService.event.on(
      'showUserOptionsOverlay',
      async (post: IPost) => {
        this.userOptionsOverlay = true;
        await this.setPost(post);
      },
    );
  }

  @HostListener('window:resize')
  onResize(): void {
    if (this.overlayService.followDiv && this.listDiv) {
      const position = this.overlayService.followDiv.getBoundingClientRect();
      this.listDiv.nativeElement.style.left =
        position.left - this.listDiv.nativeElement.clientWidth + 25 + 'px';
      this.listDiv.nativeElement.style.top =
        document.documentElement.scrollTop + position.top + 'px';
    }
  }

  closeOverlay(): void {
    this.post = undefined;
    this.overlayService.followDiv = undefined;
    this.optionsOverlay = false;
    this.replyOverlay = false;
    this.userOptionsOverlay = false;
  }

  deletePost(): void {
    this.postService.deletePost(this.post.id);
  }

  repostPost(): void {
    this.repostService.sendRepost(this.post.id);
  }

  repostWithComment(): void {
    const dialogRef = this.dialog.open(PostFormComponent, {
      width: '50%',
    });
    dialogRef.componentInstance.repost = this.post;
    dialogRef.componentInstance.user = this.post.user;
  }

  copyLinkToPost(): void {
    this.clipboard.copyFromContent(
      window.location.origin + '/status/' + this.post.id,
    );
  }

  addToBookmarks(): void {
    this.bookmarkService.postBookmark(this.post.bookmarkId);
  }

  removeBookmark(): void {
    this.bookmarkService.removeBookmark(this.post.bookmarkId);
  }

  private async setPost(post: IPost): Promise<void> {
    this.post = post;
    await new Promise((resolve: any) => setTimeout(resolve, 1));
    this.onResize();
    this.overlayDiv.nativeElement.addEventListener('click', () => {
      this.closeOverlay();
    });
  }
}
