import { Component } from '@angular/core';
import { Location } from '@angular/common';
import { AuthService } from '../../../lib/auth.service';
import { NotificationService } from '../../../lib/notification.service';
import { Alert } from '../../notification/notification.component';

@Component({
  selector: 'app-logout',
  standalone: false,
  templateUrl: './logout.component.html'
})
export class LogoutComponent {
  constructor(
    private location: Location,
    private authService: AuthService,
    private notificationService: NotificationService
  ) { }

  async onSubmit() {
    try {
      await this.authService.signOut();
    } catch (e) {
      const alert: Alert = {
        type: 'danger',
        message: e instanceof Error ? e.message : 'An unexpected error occurred.'
      };
      this.notificationService.notify(alert);
    }
  }

  back(): void {
    this.location.back();
  }
}
