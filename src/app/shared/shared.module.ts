import { PortalModule } from '@angular/cdk/portal';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatProgressSpinnerModule } from '@angular/material';
import { RouterModule } from '@angular/router';
import { NgxEmojiPickerModule } from 'ngx-emoji-picker';
import { LightboxModule } from 'ngx-lightbox';
import { CloseDialogComponent } from './close-dialog/close-dialog.component';
import { PostContentComponent } from './post-content/post-content.component';
import { PollComponent } from './post-form/poll/poll.component';
import { PostFormComponent } from './post-form/post-form.component';
import { PostTextareaDirective } from './post-form/post-textarea.directive';
import { PostListComponent } from './post-list/post-list.component';
import { PostNumberComponent } from './post-number/post-number.component';
import { PostPinnedComponent } from './post-pinned/post-pinned.component';
import { PostComponent } from './post/post.component';
import { TimeDirective } from './post/time.directive';
import { RepostComponent } from './repost/repost.component';
import { RightSideComponent } from './right-side/right-side.component';
import { TopComponent } from './top/top.component';

@NgModule({
  declarations: [
    RightSideComponent,
    PostComponent,
    PostFormComponent,
    PollComponent,
    TimeDirective,
    PostContentComponent,
    PostPinnedComponent,
    RepostComponent,
    PostNumberComponent,
    TopComponent,
    PostListComponent,
    CloseDialogComponent,
    PostTextareaDirective,
  ],
  entryComponents: [PostFormComponent, CloseDialogComponent],
  imports: [
    CommonModule,
    RouterModule,
    NgxEmojiPickerModule,
    LightboxModule,
    MatProgressSpinnerModule,
    PortalModule,
  ],
  exports: [
    PostComponent,
    PostFormComponent,
    PostContentComponent,
    PostPinnedComponent,
    PostListComponent,
    RightSideComponent,
    RepostComponent,
    PollComponent,
    TopComponent,
    TimeDirective,
  ],
})
export class SharedModule {}
