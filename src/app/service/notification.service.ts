import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { INotification } from '../model/notification.model';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  constructor(private readonly http: HttpClient) {}

  public getNotifications(): Observable<INotification[]> {
    return this.http.get<INotification[]>(`${environment.API_URL}notification`);
  }
}
