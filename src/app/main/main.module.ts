import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { BookmarkResolver } from '../resolver/bookmark.resolver';
import { NotificationResolver } from '../resolver/notification.resolver';
import {
  PostRepliesResolver,
  PostResolver,
  PostSearchResolver,
} from '../resolver/post.resolver';
import { SharedModule } from '../shared/shared.module';
import { BookmarkComponent } from './bookmark/bookmark.component';
import { HomeComponent } from './home/home.component';
import { MainRoutingModule } from './main-routing.module';
import { NotificationComponent } from './notification/notification.component';
import { StatusComponent } from './status/status.component';

@NgModule({
  declarations: [
    HomeComponent,
    NotificationComponent,
    StatusComponent,
    BookmarkComponent,
  ],
  imports: [CommonModule, MainRoutingModule, SharedModule],
  providers: [
    PostResolver,
    PostRepliesResolver,
    PostSearchResolver,
    BookmarkResolver,
    NotificationResolver,
  ],
})
export class MainModule {}
