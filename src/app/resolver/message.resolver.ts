import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Observable, of } from 'rxjs';
import { IChat } from '../model/message.model';
import { MessageService } from '../service/message.service';
import { MessageStorage } from '../storage/message.storage';

@Injectable({ providedIn: 'root' })
export class ChatResolver implements Resolve<IChat[]> {
  constructor(
    private readonly messageService: MessageService,
    private readonly messageStorage: MessageStorage,
  ) {}

  resolve(): Observable<IChat[]> {
    if (!this.messageStorage.chats) {
      return this.messageService.getChats();
    }
    return of([]);
  }
}
