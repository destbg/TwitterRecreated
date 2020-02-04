import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { ColorEvent } from 'ngx-color';
import { HistoryStorage } from 'src/app/storage/history.storage';
import { MessageStorage } from 'src/app/storage/message.storage';
import { AuthService } from 'src/app/service/auth.service';

@Component({
  selector: 'app-chat-color',
  templateUrl: './chat-color.component.html',
  styleUrls: ['../chat-options/chat-options.component.scss'],
})
export class ChatColorComponent implements OnInit {
  private username: string;
  selfColor = '#4b0082';
  othersColor = '#8b0000';

  constructor(
    private readonly route: ActivatedRoute,
    private readonly authService: AuthService,
    public messageStorage: MessageStorage,
    public historyStorage: HistoryStorage,
  ) {}

  ngOnInit(): void {
    if (this.authService.isAuthenticated()) {
      this.username = this.authService.currentUserValue.username;
    }
    this.route.paramMap.subscribe((paramMap: ParamMap) => {
      const groupId = +paramMap.get('id');
      if (
        !this.messageStorage.selectedChat ||
        this.messageStorage.selectedChat.id !== groupId
      ) {
        this.messageStorage.selectChat(groupId);
      }
      const chat = this.messageStorage.selectedChat;
      if (chat) {
        const chatUser = chat.users.find(f => f.username === this.username);
        if (chatUser) {
          this.othersColor = chatUser.othersColor;
          this.selfColor = chatUser.selfColor;
        }
      }
    });
  }

  changedColor(event: ColorEvent, self: boolean): void {
    if (self) {
      this.selfColor = event.color.hex;
    } else {
      this.othersColor = event.color.hex;
    }
  }

  revertToDefault(): void {
    this.selfColor = '#4b0082';
    this.othersColor = '#8b0000';
  }

  saveChanges(): void {
    this.messageStorage.changeColor(this.selfColor, this.othersColor);
    this.historyStorage.navigateBack();
  }
}
