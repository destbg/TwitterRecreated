import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import {
  INotification,
  NotificationType,
} from 'src/app/model/notification.model';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
})
export class NotificationComponent implements OnInit {
  notifications: INotification[];
  notificationType = NotificationType;

  constructor(
    private readonly title: Title,
    private readonly route: ActivatedRoute,
  ) {}

  ngOnInit(): void {
    this.title.setTitle('Notifications | AngularTwitter');
    this.notifications = this.route.snapshot.data.notifications;
  }
}
