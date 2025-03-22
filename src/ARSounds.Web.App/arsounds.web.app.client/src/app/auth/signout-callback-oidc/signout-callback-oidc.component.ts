import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth/auth.service';

@Component({
  selector: 'app-signout-callback-oidc',
  template: ''
})
export class SignOutCallbackOidcComponent implements OnInit {

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit() {
    this.authService.completeSignOut().then((value) => {
      this.router.navigateByUrl('/login');
    });
  }
}
