import { Injectable } from '@angular/core';
import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
  HttpTransportType,
} from '@aspnet/signalr';
import { environment } from 'src/environments/environment';
import { ICallResponse, IRequestCall } from '../model/message.model';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class SocketService {
  private readonly _hubConnection: HubConnection;

  get hubConnection(): HubConnection {
    return this._hubConnection;
  }

  constructor(private readonly authService: AuthService) {
    this._hubConnection = new HubConnectionBuilder()
      .withUrl(environment.API_URL.replace('api/', '') + 'main', {
        accessTokenFactory: () => this.authService.currentUserTokenValue,
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets,
      })
      .build();
  }

  public async connectToServer(): Promise<void> {
    if (this._hubConnection.state === HubConnectionState.Disconnected) {
      await this._hubConnection
        .start()
        .catch(err => console.log('Error while starting connection: ' + err));
    }
  }

  public async followPosts(postIds: number[]): Promise<void> {
    while (this._hubConnection.state !== HubConnectionState.Connected) {
      await new Promise(resolve => setTimeout(resolve, 100));
    }
    this._hubConnection.send('followPosts', postIds);
  }

  public async unFollowPosts(postIds: number[]): Promise<void> {
    while (this._hubConnection.state !== HubConnectionState.Connected) {
      await new Promise(resolve => setTimeout(resolve, 100));
    }
    this._hubConnection.send('unFollowPosts', postIds);
  }

  public async messagesRead(chatId: number): Promise<void> {
    while (this._hubConnection.state !== HubConnectionState.Connected) {
      await new Promise(resolve => setTimeout(resolve, 100));
    }
    this._hubConnection.send('messagesRead', { chatId });
  }

  public async startTyping(chatId: number): Promise<void> {
    while (this._hubConnection.state !== HubConnectionState.Connected) {
      await new Promise(resolve => setTimeout(resolve, 100));
    }
    this._hubConnection.send('startTyping', { chatId });
  }

  public async requestCall(chat: IRequestCall): Promise<void> {
    while (this._hubConnection.state !== HubConnectionState.Connected) {
      await new Promise(resolve => setTimeout(resolve, 100));
    }
    this._hubConnection.send('requestCall', chat);
  }

  public async respondToCall(chat: ICallResponse): Promise<void> {
    while (this._hubConnection.state !== HubConnectionState.Connected) {
      await new Promise(resolve => setTimeout(resolve, 100));
    }
    this._hubConnection.send('respondToCall', chat);
  }

  public async addUserToChat(username: string, chatId: number): Promise<void> {
    while (this._hubConnection.state !== HubConnectionState.Connected) {
      await new Promise(resolve => setTimeout(resolve, 100));
    }
    this._hubConnection.send('addUserToChat', { username, chatId });
  }
}
