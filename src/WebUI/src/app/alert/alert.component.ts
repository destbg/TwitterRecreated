import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { Alert, AlertType } from '../model/alert.model';
import { AlertService } from '../service/alert.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-alert',
  templateUrl: './alert.component.html',
})
export class AlertComponent implements OnInit, OnDestroy {
  alerts: Alert[] = [];
  subscription: Subscription;
  API_URL = environment.API_URL.replace('api/', '');

  constructor(private readonly alertService: AlertService) {}

  ngOnInit(): void {
    this.subscription = this.alertService
      .onAlert()
      .subscribe((alert: Alert) => {
        if (!alert.message) {
          // clear alerts when an empty alert is received
          this.alerts = [];
          return;
        }
        if (
          this.alerts.some(
            (s: Alert) => alert.alertId && s.alertId === alert.alertId,
          )
        ) {
          return;
        }
        setTimeout(() => this.removeAlert(alert), 10000);
        // add alert to array
        this.alerts.push(alert);
      });
  }

  ngOnDestroy(): void {
    // unsubscribe to avoid memory leaks
    this.subscription.unsubscribe();
  }

  removeAlert(alert: Alert): void {
    // remove specified alert from array
    this.alerts = this.alerts.filter((x: Alert) => x !== alert);
  }

  cssClass(alert: Alert): string {
    if (!alert) {
      return;
    }

    // return css class based on alert type
    switch (alert.type) {
      case AlertType.Success:
        return 'alert alert-success';
      case AlertType.Error:
        return 'alert alert-danger';
      case AlertType.Info:
        return 'alert alert-info';
      case AlertType.Warning:
        return 'alert alert-warning';
    }
  }
}
