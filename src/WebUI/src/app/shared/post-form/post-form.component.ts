import {
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnInit,
  ViewChild,
} from '@angular/core';
import { MatDialog } from '@angular/material';
import { EmojiEvent } from 'ngx-emoji-picker';
import { IPostShort } from 'src/app/model/post.model';
import { IUserShort } from 'src/app/model/user.model';
import { AlertService } from 'src/app/service/alert.service';
import { AuthService } from 'src/app/service/auth.service';
import { PostService } from 'src/app/service/post.service';
import { GifDialogComponent } from './gif-dialog/gif-dialog.component';
import { PollComponent } from './poll/poll.component';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-post-form',
  templateUrl: './post-form.component.html',
  styleUrls: ['./post-form.component.scss'],
})
export class PostFormComponent implements OnInit {
  @Input() replyId: number;
  repost: IPostShort;
  user: IUserShort;
  submitted: boolean;
  images: string[];
  gif: string;
  video: string;
  image: string;
  poll: boolean;
  emojiToggle: boolean;
  API_URL = environment.API_URL.replace('api/', '');
  private files: FileList;
  submittedEvent = new EventEmitter();

  @ViewChild('vid', { static: false })
  private readonly vid: ElementRef<HTMLVideoElement>;
  @ViewChild('postInput', { static: false })
  private readonly postInput: ElementRef<HTMLTextAreaElement>;
  @ViewChild('appPoll', { static: false })
  private readonly appPoll: PollComponent;

  constructor(
    private readonly authService: AuthService,
    private readonly postService: PostService,
    private readonly dialog: MatDialog,
    private readonly alertService: AlertService,
  ) {
    this.poll = false;
    this.image = 'default.png';
  }

  ngOnInit(): void {
    if (!this.authService.isAuthenticated()) {
      return;
    }
    this.images = [];
    this.image = this.authService.currentUserValue.image;
  }

  async handleFileInput(files: FileList): Promise<void> {
    if (files.length > 4) {
      this.alertService.error(`you can't upload more then 4 files`);
      return;
    }

    this.video = undefined;
    this.images = [];
    this.files = files;

    for (let i = 0; i < files.length; i++) {
      const file = files.item(i);
      if (file.type.startsWith('image')) {
        const reader = new FileReader();
        reader.onload = (event: any) => {
          this.images.push(event.target.result);
        };
        reader.readAsDataURL(file);
      } else if (file.type.startsWith('video')) {
        this.video = URL.createObjectURL(file);
        await new Promise((resolve: any) => setTimeout(resolve, 1));
        this.vid.nativeElement.src = this.video;
      } else {
        alert(`You can't upload files which aren't videos or images`);
      }
    }
  }

  async sendPost(): Promise<void> {
    this.submitted = true;
    this.submittedEvent.emit();
    const value = this.postInput.nativeElement.value.trim();
    if (value === '') {
      return;
    }
    if (this.repost) {
      this.postService.sendRepost(value, this.repost.id);
    } else if (this.gif) {
      this.postService.sendPost(
        value,
        this.replyId,
        undefined,
        undefined,
        this.gif,
      );
    } else if (this.poll) {
      const polls = this.appPoll.sendPoll();
      if (polls) {
        this.postService.sendPost(
          value,
          this.replyId,
          undefined,
          this.appPoll.sendPoll(),
        );
      } else {
        return;
      }
    } else if (this.files !== undefined && this.files.length > 0) {
      this.postService.sendPost(value, this.replyId, this.files);
    } else {
      this.postService.sendPost(value, this.replyId);
    }
    this.postInput.nativeElement.value = '';
    const event = new Event('input');
    this.postInput.nativeElement.dispatchEvent(event);
    this.poll = false;
    this.video = undefined;
    this.images = [];
    this.submitted = false;
  }

  openGifDialog(): void {
    const dialogRef = this.dialog.open(GifDialogComponent, {
      width: '50%',
      height: '500px',
    });

    dialogRef.afterClosed().subscribe((gif: string) => {
      if (gif) {
        this.images = [];
        this.images.push(gif);
        this.gif = gif;
      }
    });
  }

  openPoll(): void {
    this.poll = !this.poll;
  }

  removeImage(image: string): void {
    this.images = this.images.filter((f: string) => f !== image);
    if (this.gif === image) {
      this.gif = undefined;
    }
  }

  removeVideo(): void {
    URL.revokeObjectURL(this.video);
    this.video = undefined;
  }

  handleSelection(event: EmojiEvent): void {
    this.postInput.nativeElement.value += event.char;
  }
}
