import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './auth/auth.guard';
import { MainComponent } from './main/main.component';
import { MessageComponent } from './message/message.component';
import { ProfileComponent } from './profile/profile.component';
import { ChatResolver } from './resolver/message.resolver';
import { UserResolver } from './resolver/user.resolver';
import { SearchComponent } from './search/search.component';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    loadChildren: () => import('./main/main.module').then(t => t.MainModule),
    canActivate: [AuthGuard],
  },
  {
    path: 'auth',
    loadChildren: () => import('./auth/auth.module').then(t => t.AuthModule),
  },
  {
    path: 'message',
    component: MessageComponent,
    loadChildren: () =>
      import('./message/message.module').then(t => t.MessageModule),
    canActivate: [AuthGuard],
    resolve: {
      chats: ChatResolver,
    },
  },
  {
    path: 'search/:q',
    component: SearchComponent,
    loadChildren: () =>
      import('./search/search.module').then(t => t.SearchModule),
    canActivate: [AuthGuard],
  },
  {
    path: ':username',
    component: ProfileComponent,
    loadChildren: () =>
      import('./profile/profile.module').then(t => t.ProfileModule),
    runGuardsAndResolvers: 'paramsOrQueryParamsChange',
    resolve: {
      user: UserResolver,
    },
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
