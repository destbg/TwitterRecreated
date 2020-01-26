import { Component, OnInit, Input } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { IUserShort } from 'src/app/model/user.model';
import { UserService } from 'src/app/service/user.service';
import { MessageStorage } from 'src/app/storage/message.storage';
import { IChat, IChatUser } from 'src/app/model/message.model';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-users-dialog',
  templateUrl: './users-dialog.component.html',
})
export class UsersDialogComponent implements OnInit {
  @Input() username: string;
  users: IUserShort[];
  isChat: boolean;
  API_URL = environment.API_URL.replace('api/', '');
  private usernames: string[];
  private usersArray: IUserShort[];

  constructor(
    private readonly messageStorage: MessageStorage,
    private readonly userService: UserService,
    private readonly dialogRef: MatDialogRef<UsersDialogComponent>,
  ) {
    this.isChat = true;
    this.usernames = [];
  }

  ngOnInit(): void {
    this.userService
      .getFollowingFollowers()
      .subscribe((users: IUserShort[]) => {
        this.users = users;
        this.usersArray = users;
      });
  }

  addUser(user: IUserShort): void {
    this.usernames.push(user.username);
    this.users.splice(this.users.indexOf(user), 1);
  }

  chooseUser(user: IUserShort): void {
    this.dialogRef.close({ users: [user.username], isGroup: false });
  }

  filterUsers(): IUserShort[] {
    if (!this.messageStorage.chats) {
      return this.usersArray;
    }
    const chatUsers = this.messageStorage.chats
      .filter((chat: IChat) => !chat.isGroup)
      .map((chat: IChat) => {
        const otherUser = chat.users.find(
          (user: IChatUser) => user.username !== this.username,
        );
        return otherUser;
      });
    return this.usersArray.filter((user: IUserShort) =>
      chatUsers.some(
        (chatUser: IChatUser) => chatUser.username !== user.username,
      ),
    );
  }

  setToChat(chatBtn: HTMLButtonElement, groupBtn: HTMLButtonElement): void {
    this.isChat = true;
    this.usernames = [];
    chatBtn.classList.add('active');
    groupBtn.classList.remove('active');
  }

  setToGroup(chatBtn: HTMLButtonElement, groupBtn: HTMLButtonElement): void {
    this.isChat = false;
    this.usernames = [];
    chatBtn.classList.remove('active');
    groupBtn.classList.add('active');
  }

  closeDialog(): void {
    this.dialogRef.close({ users: this.usernames, isGroup: true });
  }
}
