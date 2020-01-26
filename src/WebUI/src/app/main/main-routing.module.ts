import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BookmarkResolver } from '../resolver/bookmark.resolver';
import { NotificationResolver } from '../resolver/notification.resolver';
import { PostRepliesResolver, PostResolver } from '../resolver/post.resolver';
import { BookmarkComponent } from './bookmark/bookmark.component';
import { HomeComponent } from './home/home.component';
import { NotificationComponent } from './notification/notification.component';
import { StatusComponent } from './status/status.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full',
  },
  {
    path: 'home',
    component: HomeComponent,
  },
  {
    path: 'notification',
    component: NotificationComponent,
    resolve: {
      notifications: NotificationResolver,
    },
  },
  {
    path: 'bookmark',
    component: BookmarkComponent,
    resolve: {
      posts: BookmarkResolver,
    },
  },
  {
    path: 'status/:id',
    component: StatusComponent,
    runGuardsAndResolvers: 'paramsOrQueryParamsChange',
    resolve: {
      post: PostResolver,
      posts: PostRepliesResolver,
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MainRoutingModule {}
