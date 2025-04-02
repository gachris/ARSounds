import { Component, OnInit } from '@angular/core';
import { NotificationService } from '../../services/targets/notification.service';

export interface Alert {
  type: string;
  message: string;
}

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html'
})

export class NotificationComponent implements OnInit {
  alerts: Array<Alert> = [];

  constructor(private notificationService: NotificationService) {
  }

  ngOnInit(): void {
    this.notificationService.add.subscribe(alert => this.add(alert));
  }

  close(alert: Alert) {
    this.alerts.splice(this.alerts.indexOf(alert), 1);
  }

  add(alert: Alert) {
    setTimeout(() => this.close(alert), 5000);
    this.alerts.push(alert);
  }
}
