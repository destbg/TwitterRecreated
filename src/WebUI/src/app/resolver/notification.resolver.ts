import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Observable } from 'rxjs';
import { INotification } from '../model/notification.model';
import { NotificationService } from '../service/notification.service';

@Injectable({ providedIn: 'root' })
export class NotificationResolver implements Resolve<INotification[]> {
  constructor(private readonly notificationService: NotificationService) {}

  resolve(): Observable<INotification[]> {
    return this.notificationService.getNotifications();
  }
}
