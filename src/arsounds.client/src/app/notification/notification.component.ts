import { Component, OnInit } from '@angular/core';
import { NotificationService } from '../../lib/notification.service';

export interface Alert {
  type: string;
  message: string;
}

@Component({
  selector: 'app-notification',
  standalone: false,
  templateUrl: './notification.component.html',
  styleUrl: './notification.component.css'
})
export class NotificationComponent implements OnInit {
  alerts: Array<Alert> = [];

  constructor(private notificationService: NotificationService) { }

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
