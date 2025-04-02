import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../../services/auth/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})

export class LoginComponent implements OnInit {

  constructor(private authService: AuthService, private route: ActivatedRoute) {

  }

  ngOnInit() {
  }

  onSubmit() {
    this.route.queryParams.subscribe(params => {
      let return_url = params['return_url'];
      this.authService.signIn(return_url);
    });
  }
}
