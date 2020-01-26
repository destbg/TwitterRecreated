import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {
  PostImageSearchResolver,
  PostSearchResolver,
  PostVideoSearchResolver,
} from '../resolver/post.resolver';
import { UserSearchResolver } from '../resolver/user.resolver';
import { PostsComponent } from './posts/posts.component';
import { ImageComponent } from './image/image.component';
import { VideoComponent } from './video/video.component';
import { UsersComponent } from './users/users.component';

const routes: Routes = [
  {
    path: '',
    component: PostsComponent,
    runGuardsAndResolvers: 'paramsOrQueryParamsChange',
    resolve: {
      posts: PostSearchResolver,
    },
  },
  {
    path: 'image',
    component: ImageComponent,
    runGuardsAndResolvers: 'paramsOrQueryParamsChange',
    resolve: {
      posts: PostImageSearchResolver,
    },
  },
  {
    path: 'video',
    component: VideoComponent,
    runGuardsAndResolvers: 'paramsOrQueryParamsChange',
    resolve: {
      posts: PostVideoSearchResolver,
    },
  },
  {
    path: 'user',
    component: UsersComponent,
    runGuardsAndResolvers: 'paramsOrQueryParamsChange',
    resolve: {
      users: UserSearchResolver,
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SearchRoutingModule {}
