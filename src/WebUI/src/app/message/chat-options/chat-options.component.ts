import { Component, OnInit } from '@angular/core';
import { MessageStorage } from 'src/app/storage/message.storage';
import { HistoryStorage } from 'src/app/storage/history.storage';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { SocketService } from 'src/app/service/socket.service';
import { MatDialog } from '@angular/material';
import { AddUserDialogComponent } from '../add-user-dialog/add-user-dialog.component';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-chat-options',
  templateUrl: './chat-options.component.html',
  styleUrls: ['./chat-options.component.scss'],
})
export class ChatOptionsComponent implements OnInit {
  API_URL = environment.API_URL.replace('api/', '');

  constructor(
    private readonly route: ActivatedRoute,
    private readonly socketService: SocketService,
    private readonly dialog: MatDialog,
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
    });
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(AddUserDialogComponent, {
      minWidth: '400px',
    });

    dialogRef.afterClosed().subscribe((username: string) => {
      if (!username) {
        return;
      }
      this.socketService.addUserToChat(
        username,
        this.messageStorage.selectedChat.id,
      );
    });
  }
}
