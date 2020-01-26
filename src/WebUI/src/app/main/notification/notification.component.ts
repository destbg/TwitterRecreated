import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import {
  INotification,
  NotificationType,
} from 'src/app/model/notification.model';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
})
export class NotificationComponent implements OnInit {
  notifications: INotification[];
  notificationType = NotificationType;
  API_URL = environment.API_URL.replace('api/', '');

  constructor(
    private readonly title: Title,
    private readonly route: ActivatedRoute,
  ) {}

  ngOnInit(): void {
    this.title.setTitle('Notifications | AngularTwitter');
    this.notifications = this.route.snapshot.data.notifications;
  }
}
