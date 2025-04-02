import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth/auth.service';

@Component({
  selector: 'app-signin-callback-oidc',
  template: ''
})
export class SignInOidcCallbackComponent implements OnInit {

  constructor(private authService: AuthService, private router: Router) { }

  async ngOnInit() {
    try {
      await this.authService.completeSignIn();

      const returnUrl = localStorage.getItem('return_url') || '/';
      localStorage.removeItem('return_url');

      this.router.navigateByUrl(returnUrl);
    } catch (err) {
      console.error('Error completing sign-in:', err);
      this.router.navigate(['/login']);
    }
  }
}
