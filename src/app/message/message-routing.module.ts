import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ChatColorComponent } from './chat-color/chat-color.component';
import { ChatOptionsComponent } from './chat-options/chat-options.component';
import { ChatMessagesComponent } from './chat-messages/chat-messages.component';

const routes: Routes = [
  {
    path: ':id',
    component: ChatMessagesComponent,
  },
  {
    path: ':id/options',
    component: ChatOptionsComponent,
  },
  {
    path: ':id/color',
    component: ChatColorComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MessageRoutingModule {}
