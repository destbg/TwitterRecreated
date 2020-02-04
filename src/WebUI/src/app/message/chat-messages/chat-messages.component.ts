import {
  AfterViewInit,
  Component,
  ElementRef,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { DeviceDetectorService } from 'ngx-device-detector';
import { EmojiEvent } from 'ngx-emoji-picker';
import { interval, Subscription } from 'rxjs';
import { IChatUser, IMessage } from 'src/app/model/message.model';
import { AuthService } from 'src/app/service/auth.service';
import { MessageService } from 'src/app/service/message.service';
import { SocketService } from 'src/app/service/socket.service';
import { HistoryStorage } from 'src/app/storage/history.storage';
import { MessageStorage } from 'src/app/storage/message.storage';
import { PeerStorage } from 'src/app/storage/peer.storage';

@Component({
  selector: 'app-chat-messages',
  templateUrl: './chat-messages.component.html',
  styleUrls: ['./chat-messages.component.scss'],
})
export class ChatMessagesComponent implements OnInit, AfterViewInit, OnDestroy {
  selfColor: string;
  othersColor: string;
  username: string;
  emojiToggle: boolean;
  private chatId: number;
  private canType: boolean;
  private timer: Subscription;
  private paramSub: Subscription;
  private processing: boolean;
  private isMobile: boolean;

  @ViewChild('textInput', { static: false })
  private readonly textInput: ElementRef<HTMLInputElement>;
  @ViewChild('messagesDiv', { static: false })
  private readonly messagesDiv: ElementRef<HTMLDivElement>;

  constructor(
    private readonly deviceDetector: DeviceDetectorService,
    private readonly messageService: MessageService,
    private readonly route: ActivatedRoute,
    private readonly authService: AuthService,
    private readonly socketService: SocketService,
    private readonly peerStorage: PeerStorage,
    public messageStorage: MessageStorage,
    public historyStorage: HistoryStorage,
  ) {}

  ngOnInit(): void {
    if (!this.authService.currentUserValue) {
      return;
    }
    this.isMobile =
      this.deviceDetector.isMobile() || this.deviceDetector.isTablet();
    this.username = this.authService.currentUserValue.username;
    this.paramSub = this.route.paramMap.subscribe((paramMap: ParamMap) => {
      this.chatId = +paramMap.get('id');
      if (
        !this.messageStorage.selectedChat ||
        this.messageStorage.selectedChat.id !== this.chatId
      ) {
        this.messageService
          .getMessages(this.chatId)
          .subscribe((messages: IMessage[]) =>
            this.messageStorage.assignNewMessages(messages),
          );
        this.messageStorage.selectChat(this.chatId);
      } else if (
        this.messageStorage.selectedChat &&
        !this.messageStorage.messages
      ) {
        this.messageService
          .getMessages(this.chatId)
          .subscribe((messages: IMessage[]) =>
            this.messageStorage.assignNewMessages(messages),
          );
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
    this.timer = interval(3 * 1000).subscribe(() => {
      this.canType = true;
    });
  }

  ngAfterViewInit(): void {
    this.adjustTextArea();
    this.messageStorage.event.subscribe(async () => {
      await new Promise((resolve: any) => setTimeout(resolve, 1));
      this.scrollToBottom();
    });
    if (this.messagesDiv) {
      this.scrollToBottom();
      this.messageStorage.messagesDiv = this.messagesDiv.nativeElement;
      this.messagesDiv.nativeElement.onscroll = () => {
        this.onScroll();
      };
      this.messagesDiv.nativeElement.ontouchend = () => {
        this.onScroll();
      };
    }
  }

  ngOnDestroy(): void {
    this.timer.unsubscribe();
    this.paramSub.unsubscribe();
  }

  onScroll(): void {
    const div = this.messagesDiv.nativeElement;
    if (
      div.scrollTop < 500 &&
      !this.messageStorage.hasReachedEnd &&
      !this.processing
    ) {
      this.processing = true;
      this.messageService
        .getMessages(this.chatId, this.messageStorage.messages[0].createdAt)
        .subscribe((messages: IMessage[]) => {
          this.messageStorage.messages = messages;
          this.processing = false;
        });
    }
  }

  sendMessage(): void {
    const value = this.textInput.nativeElement.value.trim();
    if (value.length === 0) {
      return;
    }
    if (value.length > 250) {
      alert('Max length of a message is 250 characters');
      return;
    }
    this.messageService.sendMessage(this.chatId, value);
    this.messageStorage.addPreviewMessage(value);
    this.textInput.nativeElement.value = '';
    this.textChange();
  }

  keyDown(event: KeyboardEvent): void {
    if (!this.isMobile && event.key === 'Enter' && !event.shiftKey) {
      this.sendMessage();
      event.preventDefault();
    }
  }

  textChange(): void {
    if (this.textInput.nativeElement.value.length !== 0 && this.canType) {
      this.socketService.startTyping(this.chatId);
      this.canType = false;
    }
    this.adjustTextArea();
  }

  joinUsersTyping(): string {
    const users = this.messageStorage.usersTyping.map((user: string) =>
      user.length > 10 ? user.slice(0, 10) + '...' : user,
    );
    if (users.length === 1) {
      return users[0] + ' is';
    }
    if (users.length < 3) {
      return users.join(', ') + ' are';
    }
    return users.slice(0, 3).join(', ') + `and ${users.length - 3} others are`;
  }

  handleSelection(event: EmojiEvent): void {
    this.textInput.nativeElement.value += event.char;
    this.textChange();
  }

  getChatUser(): IChatUser {
    return this.messageStorage.selectedChat.users.find(
      (user: IChatUser) => user.username !== this.username,
    );
  }

  joinUsers(): string {
    return this.messageStorage.selectedChat.users
      .map((user: IChatUser) => user.displayName)
      .join(', ');
  }

  callUser(): void {
    this.peerStorage.requestingCall.emit(this.chatId);
  }

  private adjustTextArea(): void {
    if (!this.textInput) {
      return;
    }
    this.textInput.nativeElement.style.height = 'auto';
    if (this.textInput.nativeElement.scrollHeight < 70) {
      this.textInput.nativeElement.style.height =
        this.textInput.nativeElement.scrollHeight + 'px';
    }
  }

  private scrollToBottom(): void {
    try {
      this.messagesDiv.nativeElement.scrollTo(
        0,
        this.messagesDiv.nativeElement.scrollHeight,
      );
    } catch {
      this.messagesDiv.nativeElement.scrollTop = this.messagesDiv.nativeElement.scrollHeight;
    }
  }
}
