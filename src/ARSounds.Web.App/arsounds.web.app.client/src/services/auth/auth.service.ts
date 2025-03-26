import { Injectable } from '@angular/core';

import { UserManager, UserManagerSettings, User } from 'oidc-client';
import { Observable, Subject } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private userLoginSubject = new Subject<boolean>();
  private manager = new UserManager(getClientSettings());
  private user: User = null;

  constructor() {
    this.user = JSON.parse(localStorage.getItem('user'));

    if (this.user == null) {
      this.manager.getUser().then(user => {
        this.user = user;
      });
    }
    else if (!this.isLoggedIn()) {
      this.user = null;
    }
  }

  getUserLoggedInEvents(): Observable<boolean> {
    return this.userLoginSubject.asObservable();
  }

  isLoggedIn(): boolean {
    return this.user != null && !this.user.expired;
  }

  getClaims(): any {
    return this.user.profile;
  }

  getAuthorizationHeaderValue(): string {
    return `${this.user.token_type} ${this.user.access_token}`;
  }

  signIn(returnUrl: string): Promise<void> {
    if (returnUrl === undefined || returnUrl === null) {
      returnUrl = '/';
    }
    localStorage.setItem("return_url", returnUrl);
    return this.manager.signinRedirect({
      extraQueryParams: {
        return_url: returnUrl,
        client: 'arsounds'
      }
    });
  }

  completeSignIn(): Promise<void> {
    return this.manager.signinRedirectCallback().then(user => {
      this.user = user;
      localStorage.setItem('user', this.user.toStorageString());
      this.userLoginSubject.next(this.isLoggedIn());
    });
  }

  signOut(): Promise<void> {
    localStorage.removeItem("user");
    return this.manager.signoutRedirect();
  }

  completeSignOut(): Promise<void> {
    return this.manager.signoutRedirectCallback().then(response => {
      this.user = null;
      this.userLoginSubject.next(this.isLoggedIn());
    });
  }
}

export function getClientSettings(): UserManagerSettings {
  return {
    authority: environment.authority,
    client_id: environment.client_id,
    redirect_uri: environment.redirect_uri,
    silent_redirect_uri: environment.silent_redirect_uri,
    post_logout_redirect_uri: environment.post_logout_redirect_uri,
    response_type: "code",
    scope: "openid profile email roles offline_access arsounds.read arsounds.write",
    filterProtocolClaims: true,
    loadUserInfo: true,
    automaticSilentRenew: true,
  };
}
