import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import {
  PostImageSearchResolver,
  PostSearchResolver,
  PostVideoSearchResolver,
} from '../resolver/post.resolver';
import { UserSearchResolver } from '../resolver/user.resolver';
import { SharedModule } from '../shared/shared.module';
import { ImageComponent } from './image/image.component';
import { PostsComponent } from './posts/posts.component';
import { SearchRoutingModule } from './search-routing.module';
import { UsersComponent } from './users/users.component';
import { VideoComponent } from './video/video.component';

@NgModule({
  declarations: [
    PostsComponent,
    VideoComponent,
    UsersComponent,
    ImageComponent,
  ],
  imports: [CommonModule, SearchRoutingModule, SharedModule],
  providers: [
    PostSearchResolver,
    PostImageSearchResolver,
    PostVideoSearchResolver,
    UserSearchResolver,
  ],
})
export class SearchModule {}
