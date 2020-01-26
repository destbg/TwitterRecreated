import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IChat, IMessage } from '../model/message.model';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  constructor(private readonly http: HttpClient) {}

  public getMessages(id: number, skip?: Date): Observable<IMessage[]> {
    if (!skip) {
      return this.http.get<IMessage[]>(`${environment.API_URL}message/${id}`);
    }
    return this.http.get<IMessage[]>(
      `${environment.API_URL}message/${id}/${skip}`,
    );
  }

  public sendMessage(chatId: number, content: string): void {
    this.http
      .post(`${environment.API_URL}message`, {
        content,
        chatId,
      })
      .subscribe((data: any) => data);
  }

  public getChats(): Observable<IChat[]> {
    return this.http.get<IChat[]>(`${environment.API_URL}message/chat`);
  }

  public createGroupChat(users: string[]): Observable<IChat> {
    return this.http.post<IChat>(`${environment.API_URL}message/groupchat`, {
      users,
    });
  }

  public createChat(username: string): Observable<IChat | string> {
    return this.http.post<IChat | string>(
      `${environment.API_URL}message/chat`,
      {
        username,
      },
    );
  }

  public changeColors(
    chatId: number,
    selfColor: string,
    othersColor: string,
  ): void {
    this.http
      .post(`${environment.API_URL}message/color`, {
        chatId,
        selfColor,
        othersColor,
      })
      .subscribe((data: any) => data);
  }
}
