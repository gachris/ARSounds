import { Component } from '@angular/core';
import { AuthService } from '../../../services/auth/auth.service';
import { Location } from '@angular/common'

@Component({
  selector: 'app-logout',
  standalone: false,
  templateUrl: './logout.component.html'
})

export class LogoutComponent {

  constructor(private location: Location, private authService: AuthService) {
  }

  onSubmit() {
    this.authService.signOut();
  }

  back(): void {
    this.location.back()
  }

}
