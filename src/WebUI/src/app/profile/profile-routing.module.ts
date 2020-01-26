import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeProfileComponent } from './home-profile/home-profile.component';
import { LikedProfileComponent } from './liked-profile/liked-profile.component';
import { MultimediaProfileComponent } from './multimedia-profile/multimedia-profile.component';
import {
  UserPostsResolver,
  UserLikedPostsResolver,
  UserMultimediaPostsResolver,
} from '../resolver/post.resolver';

const routes: Routes = [
  {
    path: '',
    component: HomeProfileComponent,
    resolve: {
      posts: UserPostsResolver,
    },
    runGuardsAndResolvers: 'paramsOrQueryParamsChange',
  },
  {
    path: 'liked',
    component: LikedProfileComponent,
    resolve: {
      posts: UserLikedPostsResolver,
    },
  },
  {
    path: 'multimedia',
    component: MultimediaProfileComponent,
    resolve: {
      posts: UserMultimediaPostsResolver,
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ProfileRoutingModule {}
