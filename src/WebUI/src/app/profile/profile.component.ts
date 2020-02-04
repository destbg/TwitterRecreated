import { DOCUMENT } from '@angular/common';
import {
  AfterViewInit,
  Component,
  ElementRef,
  HostListener,
  Inject,
  OnInit,
  ViewChild,
} from '@angular/core';
import { MatDialog } from '@angular/material';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Data } from '@angular/router';
import { IUser } from '../model/user.model';
import { AuthService } from '../service/auth.service';
import { FollowService } from '../service/follow.service';
import { HistoryStorage } from '../storage/history.storage';
import { EditProfileDialogComponent } from './edit-profile-dialog/edit-profile-dialog.component';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
})
export class ProfileComponent implements OnInit, AfterViewInit {
  user: IUser;
  userLoaded: boolean;
  username: string;

  @ViewChild('thumbnail', { static: false })
  private readonly thumbnail: ElementRef<HTMLDivElement>;
  @ViewChild('thumbnailimg', { static: false })
  private readonly thumbnailImage: ElementRef<HTMLImageElement>;
  @ViewChild('topView', { static: false })
  private readonly topView: ElementRef<HTMLDivElement>;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly authService: AuthService,
    private readonly followService: FollowService,
    private readonly title: Title,
    private readonly dialog: MatDialog,
    public historyStorage: HistoryStorage,
    @Inject(DOCUMENT) public document: Document,
  ) {
    this.username = '';
    this.userLoaded = false;
  }

  ngOnInit(): void {
    this.route.data.subscribe(async (routerData: Data) => {
      if (this.authService.isAuthenticated()) {
        this.username = this.authService.currentUserValue.username;
      }
      const user = routerData.user;
      if (user) {
        await this.assignUser(user);
      } else {
        this.user = {
          displayName: 'User not found',
          username:
            typeof window != 'undefined'
              ? window.location.pathname.split('/')[1]
              : '',
          description: '',
          image: 'default.png',
          thumbnail: 'default.jpg',
          followed: false,
          followers: 0,
          following: 0,
        } as IUser;
        this.title.setTitle('User not found | AngularTwitter');
        await new Promise((resolve: any) => setTimeout(resolve, 1));
        this.setThumbnailSize();
      }
    });
  }

  @HostListener('window:resize')
  onResize(): void {
    this.setThumbnailSize();
  }

  ngAfterViewInit(): void {
    this.setThumbnailSize();
  }

  followUser(): void {
    this.followService.followUser(this.user.username, !this.user.followed);
    this.user.followed = !this.user.followed;
  }

  buttonCss(): string {
    return this.user.followed ? 'danger' : 'info';
  }

  openSettingsDialog(): void {
    const dialogRef = this.dialog.open(EditProfileDialogComponent, {
      width: '70%',
      maxWidth: '600px',
      height: '80%',
    });
    dialogRef.componentInstance.user = this.user;

    dialogRef
      .afterClosed()
      .subscribe(
        (data: {
          displayName: string;
          description: string;
          image: string;
          thumbnail: string;
        }) => {
          if (data.thumbnail) {
            this.user.thumbnail = data.thumbnail;
          }
          if (data.image) {
            this.user.image = data.image;
          }
          this.user.displayName = data.displayName;
          this.user.description = data.description;
        },
      );
  }

  private async assignUser(user: IUser): Promise<void> {
    this.user = user;
    this.userLoaded = true;
    this.title.setTitle(`${user.username} | AngularTwitter`);
    await new Promise((resolve: any) => setTimeout(resolve, 1));
    this.setThumbnailSize();
  }

  private setThumbnailSize(): void {
    if (!this.thumbnail || !this.topView) {
      return;
    }
    this.thumbnail.nativeElement.style.height =
      this.topView.nativeElement.clientWidth / 3 + 'px';
    this.thumbnailImage.nativeElement.style.height = this.thumbnail.nativeElement.style.height;
    this.thumbnailImage.nativeElement.style.width =
      this.topView.nativeElement.clientWidth + 'px';
  }
}
