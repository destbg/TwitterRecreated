import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, Router, RouterEvent } from '@angular/router';
import { MessageStorage } from '../storage/message.storage';
import { UsersDialogComponent } from './users-dialog/users-dialog.component';
import { Subscription } from 'rxjs';
import { IChatUser, IChat } from '../model/message.model';
import { AuthService } from '../service/auth.service';
import { MessageService } from '../service/message.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.scss'],
})
export class MessageComponent implements OnInit, OnDestroy {
  API_URL = environment.API_URL.replace('api/', '');
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
          this.messageService
            .createGroupChat(response.users)
            .subscribe((chat: IChat) => {
              this.messageStorage.chats = [chat];
              window.location.reload();
            });
        } else {
          this.messageService
            .createChat(response.users[0])
            .subscribe((chat: IChat | string) => {
              if (typeof chat === 'string') {
                this.router.navigate(['/message/' + chat]);
              } else {
                this.messageStorage.chats = [chat];
                window.location.reload();
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
