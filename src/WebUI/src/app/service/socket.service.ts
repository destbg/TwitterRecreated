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

  public followPosts(postIds: number[]): void {
    this._hubConnection.send('followPosts', postIds);
  }

  public unFollowPosts(postIds: number[]): void {
    this._hubConnection.send('unFollowPosts', postIds);
  }

  public messagesRead(chatId: number): void {
    this._hubConnection.send('messagesRead', { chatId });
  }

  public startTyping(chatId: number): void {
    this._hubConnection.send('startTyping', { chatId });
  }

  public requestCall(chat: IRequestCall): void {
    this._hubConnection.send('requestCall', chat);
  }

  public respondToCall(chat: ICallResponse): void {
    this._hubConnection.send('respondToCall', chat);
  }

  public addUserToChat(username: string, chatId: number): void {
    this._hubConnection.send('addUserToChat', { username, chatId });
  }
}
