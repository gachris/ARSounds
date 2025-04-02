import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { Router, RouterModule } from '@angular/router';
import { AppRoutingModule } from './app-routing.module';
import { TargetsModule } from './targets/targets.module';
import { AuthModule } from './auth/auth.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AppComponent } from './app.component';
import { AuthGuardService } from '../services/auth/auth-guard.service';
import { AuthService } from '../services/auth/auth.service';
import { PublicComponent } from './layouts/public/public.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { FooterComponent } from './footer/footer.component';
import { SecureComponent } from './layouts/secure/secure.component';
import { NotificationComponent } from './notification/notification.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';

@NgModule({ declarations: [
        AppComponent,
        NavMenuComponent,
        FooterComponent,
        PublicComponent,
        SecureComponent,
        NotificationComponent,
        PageNotFoundComponent,
    ],
    exports: [RouterModule],
    bootstrap: [AppComponent], imports: [BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        BrowserModule,
        FormsModule,
        TargetsModule,
        AuthModule,
        AppRoutingModule,
        NgbModule], providers: [AuthGuardService, AuthService, provideHttpClient(withInterceptorsFromDi())] })
export class AppModule {
  // Diagnostic only: inspect router configuration
  constructor(router: Router) {
    // Use a custom replacer to display function names in the route configs
    // const replacer = (key, value) => (typeof value === 'function') ? value.name : value;

    // console.log('Routes: ', JSON.stringify(router.config, replacer, 2));
  }
}
