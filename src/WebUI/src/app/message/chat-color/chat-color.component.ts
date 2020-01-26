import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { ColorEvent } from 'ngx-color';
import { HistoryStorage } from 'src/app/storage/history.storage';
import { MessageStorage } from 'src/app/storage/message.storage';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-chat-color',
  templateUrl: './chat-color.component.html',
  styleUrls: ['../chat-options/chat-options.component.scss'],
})
export class ChatColorComponent implements OnInit {
  selfColor = '#4b0082';
  othersColor = '#8b0000';
  API_URL = environment.API_URL.replace('api/', '');

  constructor(
    private readonly route: ActivatedRoute,
    public messageStorage: MessageStorage,
    public historyStorage: HistoryStorage,
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((paramMap: ParamMap) => {
      const groupId = +paramMap.get('id');
      if (
        !this.messageStorage.selectedChat ||
        this.messageStorage.selectedChat.id !== groupId
      ) {
        this.messageStorage.selectChat(groupId);
      }
      const group = this.messageStorage.selectedChat;
      if (group) {
        if (group.userOptions && group.userOptions.othersColor) {
          this.othersColor = group.userOptions.othersColor;
        }
        if (group.userOptions && group.userOptions.selfColor) {
          this.selfColor = group.userOptions.selfColor;
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
