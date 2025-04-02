import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../services/auth/auth.service';

@Component({
  selector: 'app-silent-callback-oidc',
  standalone: false,
  template: ''
})
export class SilentCallbackOidcComponent implements OnInit {

  constructor(private authService: AuthService) { }

  async ngOnInit() {
    try {
      await this.authService.completeSilentSignIn();
    } catch (error) {
      console.error('Silent renew failed', error);
    }
  }
}
