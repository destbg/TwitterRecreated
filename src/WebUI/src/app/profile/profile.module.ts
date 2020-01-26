import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import {
  UserLikedPostsResolver,
  UserMultimediaPostsResolver,
  UserPostsResolver,
} from '../resolver/post.resolver';
import { SharedModule } from '../shared/shared.module';
import { HomeProfileComponent } from './home-profile/home-profile.component';
import { LikedProfileComponent } from './liked-profile/liked-profile.component';
import { MultimediaProfileComponent } from './multimedia-profile/multimedia-profile.component';
import { ProfileRoutingModule } from './profile-routing.module';

@NgModule({
  declarations: [
    HomeProfileComponent,
    LikedProfileComponent,
    MultimediaProfileComponent,
  ],
  imports: [CommonModule, ProfileRoutingModule, SharedModule],
  providers: [
    UserPostsResolver,
    UserLikedPostsResolver,
    UserMultimediaPostsResolver,
  ],
})
export class ProfileModule {}
