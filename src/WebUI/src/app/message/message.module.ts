import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ColorSketchModule } from 'ngx-color/sketch';
import { NgxEmojiPickerModule } from 'ngx-emoji-picker';
import { ChatColorComponent } from './chat-color/chat-color.component';
import { ChatOptionsComponent } from './chat-options/chat-options.component';
import { ChatMessagesComponent } from './chat-messages/chat-messages.component';
import { MessageRoutingModule } from './message-routing.module';

@NgModule({
  declarations: [
    ChatMessagesComponent,
    ChatOptionsComponent,
    ChatColorComponent,
  ],
  imports: [
    CommonModule,
    NgxEmojiPickerModule,
    ColorSketchModule,
    MessageRoutingModule,
  ],
})
export class MessageModule {}
