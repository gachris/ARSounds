import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../lib/auth.service';

@Component({
  selector: 'app-signin-callback-oidc',
  standalone: false,
  template: ''
})
export class SignInCallbackOidcComponent implements OnInit {
  constructor(
    private router: Router,
    private authService: AuthService
  ) { }

  async ngOnInit(): Promise<void> {
    try {
      await this.authService.completeSignIn();

      const returnUrl = localStorage.getItem('return_url') || '/';
      localStorage.removeItem('return_url');

      await this.router.navigateByUrl(returnUrl);
    } catch (error) {
      console.error('Error completing sign-in:', error);
      await this.router.navigate(['/login']);
    }
  }
}
