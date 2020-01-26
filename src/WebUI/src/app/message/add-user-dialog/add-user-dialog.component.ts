import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { UserService } from 'src/app/service/user.service';
import { MessageStorage } from 'src/app/storage/message.storage';
import { IUserShort } from 'src/app/model/user.model';
import { IChatUser } from 'src/app/model/message.model';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-add-user-dialog',
  templateUrl: './add-user-dialog.component.html',
})
export class AddUserDialogComponent implements OnInit {
  users: IUserShort[];
  API_URL = environment.API_URL.replace('api/', '');

  constructor(
    private readonly messageStorage: MessageStorage,
    private readonly userService: UserService,
    private readonly dialogRef: MatDialogRef<AddUserDialogComponent>,
  ) {}

  ngOnInit(): void {
    this.userService
      .getFollowingFollowers()
      .subscribe((users: IUserShort[]) => {
        this.users = users.filter(
          (user: IUserShort) =>
            !this.messageStorage.selectedChat.users.some(
              (chatUser: IChatUser) => chatUser.username === user.username,
            ),
        );
      });
  }

  addUser(index: number): void {
    this.dialogRef.close(this.users[index].username);
  }

  closeDialog(): void {
    this.dialogRef.close();
  }
}
