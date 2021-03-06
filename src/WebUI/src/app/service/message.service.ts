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
    return this.http.get<IChat[]>(`${environment.API_URL}chat`);
  }

  public createGroupChat(users: string[]): void {
    this.http
      .post(`${environment.API_URL}chat/group`, {
        users,
      })
      .subscribe(f => f);
  }

  public createChat(username: string): Observable<number> {
    return this.http.post<number>(`${environment.API_URL}chat`, {
      username,
    });
  }

  public changeColors(
    chatId: number,
    selfColor: string,
    othersColor: string,
  ): void {
    this.http
      .post(`${environment.API_URL}chat/color`, {
        chatId,
        selfColor,
        othersColor,
      })
      .subscribe(f => f);
  }
}
