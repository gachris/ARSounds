import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthRoutingModule } from './auth-routing.module';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { SignInCallbackOidcComponent } from './signin-callback-oidc/signin-callback-oidc.component';
import { SignOutCallbackOidcComponent } from './signout-callback-oidc/signout-callback-oidc.component';
import { SilentCallbackOidcComponent } from './silent-callback-oidc/silent-callback-oidc.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        AuthRoutingModule
    ],
    declarations: [
        LoginComponent,
        LogoutComponent,
        SignInCallbackOidcComponent,
        SignOutCallbackOidcComponent,
        SilentCallbackOidcComponent
    ]
})
export class AuthModule { }
