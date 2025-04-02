import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth/auth.service';

@Component({
  selector: 'app-signout-callback-oidc',
  standalone: false,
  template: ''
})
export class SignOutCallbackOidcComponent implements OnInit {
  constructor(private authService: AuthService, private router: Router) { }

  async ngOnInit() {
    try {
      await this.authService.completeSignOut();
      this.router.navigateByUrl('/login');
    } catch (error) {
      console.error('Sign-out callback failed:', error);
      this.router.navigateByUrl('/login');
    }
  }
}
