import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../../lib/auth.service';
import { NotificationService } from '../../../lib/notification.service';
import { Alert } from '../../notification/notification.component';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  constructor(
    private route: ActivatedRoute,
    private authService: AuthService,
    private notificationService: NotificationService
  ) { }

  async onSubmit() {
    try {
      const returnUrl = this.route.snapshot.queryParams['return_url'] || '/';
      await this.authService.signIn(returnUrl);
    } catch (e) {
      const alert: Alert = {
        type: 'danger',
        message: e instanceof Error ? e.message : 'An unexpected error occurred.'
      };
      this.notificationService.notify(alert);
    }
  }
}
