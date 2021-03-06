import { EventEmitter, Injectable } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { interval, Subscription } from 'rxjs';
import { ISelfUser } from '../model/auth.model';
import {
  IChat,
  IChatUser,
  IMessage,
  IMessageRead,
  IUserInChat,
  IUserTyping,
  PreviewMessage,
} from '../model/message.model';
import { AuthService } from '../service/auth.service';
import { MessageService } from '../service/message.service';
import { SocketService } from '../service/socket.service';

@Injectable({
  providedIn: 'root',
})
export class MessageStorage {
  private focused: boolean;
  private user: ISelfUser;
  private typeTimers: { user: string; sub: Subscription }[];
  private subscription: Subscription;
  private flashTimer: Subscription;
  private chatsArray: IChat[];
  get chats(): IChat[] {
    if (this.chatsArray.length === 0) {
      return undefined;
    }
    return this.chatsArray;
  }
  set chats(values: IChat[]) {
    this.chatsArray.push.apply(this.chatsArray, values);
  }
  private previewMessagesArray: PreviewMessage[];
  get previewMessages(): PreviewMessage[] {
    if (this.previewMessagesArray.length === 0) {
      return undefined;
    }
    return this.previewMessagesArray;
  }
  private messagesArray: IMessage[];
  get messages(): IMessage[] {
    if (this.messagesArray.length === 0) {
      return undefined;
    }
    return this.messagesArray;
  }
  set messages(values: IMessage[]) {
    if (values.length < 50) {
      this.hasReachedEnd = true;
    }
    this.checkMessagesRead();
    this.messagesArray.unshift.apply(this.messagesArray, values);
  }
  private usersTypingArray: string[];
  get usersTyping(): string[] {
    if (this.usersTypingArray.length === 0) {
      return undefined;
    }
    return this.usersTypingArray;
  }

  public event: EventEmitter<void>;
  public selectedChat: IChat;
  public messagesDiv: HTMLDivElement;
  public messagesInitialized: boolean;
  public phoneSized: boolean;
  public messageTab: boolean;
  public hasMessage: boolean;
  public hasReachedEnd: boolean;

  constructor(
    private readonly socketService: SocketService,
    private readonly messageService: MessageService,
    private readonly title: Title,
    authService: AuthService,
  ) {
    this.chatsArray = [];
    this.messagesArray = [];
    this.usersTypingArray = [];
    this.previewMessagesArray = [];
    this.typeTimers = [];
    this.messageTab = true;
    this.phoneSized = false;
    this.focused = true;
    this.event = new EventEmitter<void>();
    if (typeof window != 'undefined') {
      window.onfocus = () => {
        this.focused = true;
        if (this.hasMessage) {
          this.socketService.messagesRead(this.selectedChat.id);
          this.stopFlashTab();
          this.hasMessage = false;
        }
      };
      window.onblur = () => {
        this.focused = false;
      };
    }
    authService.currentUser.subscribe((user: ISelfUser) => {
      if (user) {
        this.user = user;
      }
      this.setEverythingToNull();
    });
    socketService.hubConnection.on('newMessage', (message: IMessage) => {
      this.handleNewMessage(message);
    });
    socketService.hubConnection.on(
      'messagesWereRead',
      (messagesRead: IMessageRead) => {
        this.handleMessagesRead(messagesRead);
      },
    );
    socketService.hubConnection.on(
      'userStartedTyping',
      (startedTyping: IUserTyping) => {
        this.handleStartedTyping(startedTyping);
      },
    );
    socketService.hubConnection.on(
      'userAddedToChat',
      (userInChat: IUserInChat) => {
        this.handleUserAddedToChat(userInChat);
      },
    );
    socketService.hubConnection.on('userNowOnline', (username: string) => {
      this.handleUserNowOnline(username);
    });
    socketService.hubConnection.on('userNowOffline', (username: string) => {
      this.handleUserNowOffline(username);
    });
    socketService.hubConnection.on('newChat', (chat: IChat) => {
      this.handleNewChat(chat);
    });
  }

  public Init(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public destroyChats(): void {
    this.subscription = interval(30 * 1000).subscribe(() => {
      this.setEverythingToNull();
    });
  }

  public selectChat(id: number): void {
    const found = this.chatsArray.find((chat: IChat) => chat.id === id);
    if (found) {
      this.selectedChat = found;
      if (found.newMessage) {
        this.socketService.messagesRead(this.selectedChat.id);
        found.newMessage = false;
      }
      this.setTitle();
      this.previewMessagesArray = [];
      this.usersTypingArray = [];
    }
  }

  public assignNewMessages(messages: IMessage[]): void {
    this.messagesArray = [];
    this.messages = messages;
  }

  public changeColor(selfColor: string, othersColor: string): void {
    if (this.selectedChat) {
      this.messageService.changeColors(
        this.selectedChat.id,
        selfColor,
        othersColor,
      );
      const chatUser = this.selectedChat.users.find(
        f => f.username === this.user.username,
      );
      if (chatUser) {
        chatUser.othersColor = othersColor;
        chatUser.selfColor = selfColor;
      }
    }
  }

  public addPreviewMessage(message: string): void {
    this.previewMessagesArray.unshift(
      new PreviewMessage(message, this.user.image),
    );
    this.checkMessagesRead();
  }

  private handleNewMessage(message: IMessage): void {
    if (this.previewMessagesArray.length > 0) {
      const previewIndex = this.previewMessagesArray.findIndex(
        (previewMessage: PreviewMessage) =>
          previewMessage.message === message.message,
      );
      if (previewIndex !== -1) {
        this.previewMessagesArray.splice(previewIndex, 1);
      }
    }
    if (!this.focused && !this.hasMessage) {
      this.playSound();
      this.flashTab();
    }
    if (this.selectedChat && message.chatId === this.selectedChat.id) {
      this.checkMessagesRead();
      this.messagesArray.push(message);
      if (!this.focused) {
        this.hasMessage = true;
      } else {
        this.socketService.messagesRead(this.selectedChat.id);
      }
    } else if (message.user.username !== this.user.username) {
      const found = this.chatsArray.find(
        (chat: IChat) => chat.id === message.chatId,
      );
      if (found) {
        found.newMessage = true;
        if (!this.focused) {
          this.hasMessage = true;
        }
      }
    }
    const index = this.chatsArray.findIndex(
      (chat: IChat) => chat.id === message.chatId,
    );
    if (index === -1) {
      return;
    }
    if (index === 0) {
      this.chatsArray[index].lastMessage = message.message;
    } else {
      const chat = this.chatsArray.splice(index, 1)[0];
      chat.lastMessage = message.message;
      this.chatsArray.unshift(chat);
    }
    this.checkUsersTyping(message.user.username);
  }

  private handleMessagesRead(messagesRead: IMessageRead): void {
    if (messagesRead.user.username === this.user.username) {
      return;
    }
    const index = this.chatsArray.findIndex(
      f =>
        f.id === messagesRead.chatId &&
        f.users.find(s => s.username === messagesRead.user.username),
    );
    if (index !== -1) {
      const userIndex = this.chatsArray[index].users.findIndex(
        f => f.username === messagesRead.user.username,
      );
      if (userIndex !== -1) {
        this.chatsArray[index].users[userIndex].messageReadId =
          messagesRead.messageId;
      }
    }
  }

  private handleStartedTyping(startedTyping: IUserTyping): void {
    if (
      this.selectedChat.id === startedTyping.chatId &&
      startedTyping.username !== this.user.username
    ) {
      this.checkUsersTyping(startedTyping.username);
      this.usersTypingArray.push(startedTyping.username);
      this.typeTimers.push({
        user: startedTyping.username,
        sub: interval(5 * 1000).subscribe(() => {
          const intervalIndex = this.typeTimers.findIndex(
            (value: { user: string }) => value.user === startedTyping.username,
          );
          if (intervalIndex !== -1) {
            this.typeTimers[intervalIndex].sub.unsubscribe();
            this.typeTimers.splice(intervalIndex, 1);
          }
          const index = this.usersTypingArray.findIndex(
            (user: string) => user === startedTyping.username,
          );
          if (index !== -1) {
            this.usersTypingArray.splice(index, 1);
          }
        }),
      });
    }
  }

  private handleUserAddedToChat(userInChat: IUserInChat): void {
    const index = this.chatsArray.findIndex(
      (chat: IChat) => chat.id === userInChat.chatId,
    );
    if (index !== -1) {
      this.chatsArray[index].users.push(userInChat.user as IChatUser);
    }
  }

  private handleUserNowOnline(username: string): void {
    const index = this.chatsArray.findIndex(f =>
      f.users.find(s => s.username === username),
    );
    if (index !== -1) {
      const userIndex = this.chatsArray[index].users.findIndex(
        f => f.username === username,
      );
      this.chatsArray[index].users[userIndex].isOnline = true;
    }
  }

  private handleUserNowOffline(username: string): void {
    const index = this.chatsArray.findIndex(f =>
      f.users.find(s => s.username === username),
    );
    if (index !== -1) {
      const userIndex = this.chatsArray[index].users.findIndex(
        f => f.username === username,
      );
      this.chatsArray[index].users[userIndex].isOnline = false;
    }
  }

  private handleNewChat(chat: IChat): void {
    this.chats = [chat];
    if (!this.focused && !this.hasMessage) {
      this.playSound();
      this.flashTab();
    }
  }

  private playSound(): void {
    const sound = document.createElement('audio');
    sound.src = 'assets/sound/notification.mp3';
    sound.setAttribute('preload', 'auto');
    sound.setAttribute('controls', 'none');
    sound.volume = 0.2;
    sound.style.display = 'none';
    document.body.appendChild(sound);
    sound.onended = () => {
      document.body.removeChild(sound);
    };
    sound.play();
  }

  private flashTab(): void {
    if (this.flashTimer) {
      return;
    }
    const currentTitle = this.title.getTitle();
    const newTitle = 'NEW MESSAGE | AngularTwitter';
    let titleType = true;
    this.flashTimer = interval(500).subscribe(() => {
      if (titleType) {
        this.title.setTitle(newTitle);
        titleType = false;
      } else {
        this.title.setTitle(currentTitle);
        titleType = true;
      }
    });
  }

  private stopFlashTab(): void {
    if (this.flashTimer) {
      this.flashTimer.unsubscribe();
      this.flashTimer = undefined;
    }
    this.setTitle();
  }

  private setTitle(): void {
    if (!this.selectedChat) {
      return;
    }
    if (this.selectedChat.name) {
      this.title.setTitle(`${this.selectedChat.name} | AngularTwitter`);
    } else {
      this.title.setTitle(
        `${
          this.selectedChat.users.find(
            (user: IChatUser) => user.username !== this.user.username,
          ).displayName
        } | AngularTwitter`,
      );
    }
  }

  private checkMessagesRead(): void {
    if (
      this.messagesDiv.scrollHeight -
        (this.messagesDiv.scrollTop + this.messagesDiv.clientHeight) <
      20
    ) {
      this.event.emit();
    }
  }

  private checkUsersTyping(username: string): void {
    const intervalIndexed = this.typeTimers.findIndex(
      (value: { user: string }) => value.user === username,
    );
    if (intervalIndexed !== -1) {
      this.typeTimers[intervalIndexed].sub.unsubscribe();
      this.typeTimers.splice(intervalIndexed, 1);
    }
    const typingIndex = this.usersTypingArray.findIndex(
      (user: string) => user === username,
    );
    if (typingIndex !== -1) {
      this.usersTypingArray.splice(typingIndex, 1);
    }
  }

  private setEverythingToNull(): void {
    this.chatsArray = [];
    this.messagesArray = [];
    this.messageTab = true;
    this.selectedChat = undefined;
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
