import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router, RouterEvent } from '@angular/router';
import { Subscription } from 'rxjs';
import { IChat, IChatUser } from '../model/message.model';
import { AuthService } from '../service/auth.service';
import { MessageService } from '../service/message.service';
import { MessageStorage } from '../storage/message.storage';
import { UsersDialogComponent } from './users-dialog/users-dialog.component';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.scss'],
})
export class MessageComponent implements OnInit, OnDestroy {
  private routerSub: Subscription;
  private username: string;

  constructor(
    private readonly messageService: MessageService,
    private readonly authService: AuthService,
    private readonly dialog: MatDialog,
    private readonly title: Title,
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    public messageStorage: MessageStorage,
  ) {}

  ngOnInit(): void {
    if (this.router.url.split('/').length === 2) {
      this.messageStorage.messageTab = false;
    } else {
      this.messageStorage.messageTab = true;
    }
    if (this.authService.isAuthenticated()) {
      this.username = this.authService.currentUserValue.username;
    }
    this.routerSub = this.router.events.subscribe(
      (routerEvent: RouterEvent) => {
        if (routerEvent.url) {
          if (routerEvent.url.split('/').length === 2) {
            this.messageStorage.messageTab = false;
          } else {
            this.messageStorage.messageTab = true;
          }
        }
      },
    );
    this.title.setTitle('Messages | AngularTwitter');
    if (this.messageStorage) {
      this.messageStorage.Init();
      if (!this.messageStorage.chats) {
        this.messageStorage.chats = this.route.snapshot.data.chats;
      }
      if (this.messageStorage.selectedChat) {
        this.router.navigate([
          '/message/' + this.messageStorage.selectedChat.id,
        ]);
      }
    }
    if (window) {
      const width = window.innerWidth;
      if (width < 768) {
        this.messageStorage.phoneSized = true;
      } else {
        this.messageStorage.phoneSized = false;
      }
      document.body.style.overflowY = 'hidden';
    }
  }

  ngOnDestroy(): void {
    if (this.routerSub) {
      this.routerSub.unsubscribe();
    }
    document.body.style.overflowY = 'visible';
    this.messageStorage.destroyChats();
  }

  onResize(): void {
    const width = window.innerWidth;
    if (width < 768) {
      this.messageStorage.phoneSized = true;
    } else {
      this.messageStorage.phoneSized = false;
    }
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(UsersDialogComponent, {
      minWidth: '400px',
    });

    dialogRef.componentInstance.username = this.username;

    dialogRef
      .afterClosed()
      .subscribe((response: { users: string[]; isGroup: boolean }) => {
        if (!response || response.users.length === 0) {
          return;
        }
        if (response.isGroup) {
          this.messageService.createGroupChat(response.users);
        } else {
          this.messageService
            .createChat(response.users[0])
            .subscribe((chatId: number) => {
              if (chatId !== undefined) {
                this.router.navigate(['/message/' + chatId]);
              }
            });
        }
      });
  }

  getChatUser(index: number): IChatUser {
    const chat = this.messageStorage.chats[index];
    return chat.users.find(
      (user: IChatUser) => user.username !== this.username,
    );
  }
}
