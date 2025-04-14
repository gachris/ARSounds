import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuardService } from '../../lib/auth-guard.service';
import { PublicComponent } from '../layouts/public/public.component';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { SignInCallbackOidcComponent } from './signin-callback-oidc/signin-callback-oidc.component';
import { SignOutCallbackOidcComponent } from './signout-callback-oidc/signout-callback-oidc.component';
import { SilentCallbackOidcComponent } from './silent-callback-oidc/silent-callback-oidc.component';

const routes: Routes = [
  {
    path: '',
    component: PublicComponent,
    data: { title: 'Public Views' },
    children: [
      {
        path: 'login',
        component: LoginComponent
      },
      {
        path: 'logout',
        component: LogoutComponent, canActivate: [AuthGuardService]
      },
      {
        path: 'signin-callback-oidc',
        component: SignInCallbackOidcComponent
      },
      {
        path: 'signout-callback-oidc',
        component: SignOutCallbackOidcComponent
      },
      {
        path: 'silent-callback-oidc',
        component: SilentCallbackOidcComponent
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthRoutingModule { }
