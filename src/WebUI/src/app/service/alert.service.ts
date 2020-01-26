import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { Alert, AlertType } from '../model/alert.model';

@Injectable({
  providedIn: 'root',
})
export class AlertService {
  private readonly subject: Subject<Alert>;

  constructor() {
    this.subject = new Subject<Alert>();
  }

  // enable subscribing to alerts observable
  onAlert(): Observable<Alert> {
    return this.subject.asObservable();
  }

  // convenience methods
  success(message: string, alertId?: string): void {
    this.alert(new Alert({ message, type: AlertType.Success, alertId }));
  }

  error(message: string, alertId?: string): void {
    this.alert(new Alert({ message, type: AlertType.Error, alertId }));
  }

  info(message: string, alertId?: string): void {
    this.alert(new Alert({ message, type: AlertType.Info, alertId }));
  }

  warn(message: string, alertId?: string): void {
    this.alert(new Alert({ message, type: AlertType.Warning, alertId }));
  }

  // main alert method
  alert(alert: Alert): void {
    this.subject.next(alert);
  }

  // clear alerts
  clear(alertId?: string): void {
    this.subject.next(new Alert({ alertId }));
  }
}
