import { EventEmitter, Injectable } from '@angular/core';
import { Alert } from '../app/notification/notification.component';

@Injectable({
  providedIn: 'root'
})

export class NotificationService {
  add: EventEmitter<Alert> = new EventEmitter();

  notify(alert: Alert) {
    this.add.emit(alert);
  }
}
