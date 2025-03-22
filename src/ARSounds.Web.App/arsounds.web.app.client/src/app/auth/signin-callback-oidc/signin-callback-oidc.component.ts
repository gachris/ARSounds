import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../../services/auth/auth.service';

@Component({
  selector: 'app-signin-callback-oidc',
  template: ''
})
export class SignInCallbackOidcComponent implements OnInit {

  constructor(private authService: AuthService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {
    const return_url = localStorage.getItem("return_url");
    this.authService.completeSignIn().then((value) => {
      this.router.navigateByUrl(return_url);
    });
  }

}
