import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../lib/auth.service';

@Component({
  selector: 'app-signout-callback-oidc',
  standalone: false,
  template: ''
})
export class SignOutCallbackOidcComponent implements OnInit {
  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  async ngOnInit(): Promise<void> {
    try {
      await this.authService.completeSignOut();
      await this.router.navigateByUrl('/login');
    } catch (error) {
      console.error('Sign-out callback failed:', error);
      await this.router.navigateByUrl('/login');
    }
  }
}
