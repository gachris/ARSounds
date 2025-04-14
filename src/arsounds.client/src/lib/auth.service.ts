import { Injectable } from '@angular/core';
import { User, UserManager, UserManagerSettings, WebStorageStateStore } from 'oidc-client';
import { Observable, BehaviorSubject } from 'rxjs';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private userManager: UserManager;
  private currentUser: User | null = null;
  private userLoginSubject = new BehaviorSubject<boolean>(false);

  constructor() {
    this.userManager = new UserManager(getClientSettings());
    this.init();
  }

  private async init(): Promise<void> {
    try {

      const user = await this.userManager.getUser();
      this.currentUser = user;
      this.userLoginSubject.next(await this.isAuthenticated());

      this.userManager.events.addUserLoaded(user => {
        this.currentUser = user;
        this.userLoginSubject.next(true);
      });

      this.userManager.events.addUserUnloaded(() => {
        this.currentUser = null;
        this.userLoginSubject.next(false);
      });

      this.userManager.events.addAccessTokenExpired(() => {
        console.warn('Access token expired');
        this.signinSilent().catch(err => {
          console.error('Silent renew failed after token expiration', err);
          this.signOut();
        });
      });

      this.userManager.events.addSilentRenewError(err => {
        console.error('Silent renew error', err);
      });

    } catch (err) {
      console.error('Failed to initialize AuthService:', err);
      this.userLoginSubject.next(false);
    }
  }

  getUserLoggedInEvents(): Observable<boolean> {
    return this.userLoginSubject.asObservable();
  }

  async isAuthenticated(): Promise<boolean> {
    const user = await this.userManager.getUser();
    this.currentUser = user;
    return user != null && !user.expired;
  }

  getClaims(): any {
    return this.currentUser?.profile ?? {};
  }

  getAuthorizationHeaderValue(): string | null {
    if (this.isAuthenticated() && this.currentUser) {
      return `${this.currentUser.token_type} ${this.currentUser.access_token}`;
    }
    return null;
  }

  async signIn(returnUrl: string = '/'): Promise<void> {
    try {
      localStorage.setItem('return_url', returnUrl);
      return await this.userManager.signinRedirect({
        extraQueryParams: {
          return_url: returnUrl
        }
      });
    } catch (err) {
      console.error('Error sign-in', err);
      throw err;
    }
  }

  async completeSignIn(): Promise<void> {
    try {
      const user = await this.userManager.signinRedirectCallback();
      this.currentUser = user;
      this.userLoginSubject.next(await this.isAuthenticated());
    } catch (err) {
      console.error('Error completing sign-in', err);
      throw err;
    }
  }

  async signOut(): Promise<void> {
    try {
      return await this.userManager.signoutRedirect();
    } catch (err) {
      console.error('Error sign-out', err);
      throw err;
    }
  }

  async completeSignOut(): Promise<void> {
    try {
      await this.userManager.signoutRedirectCallback();
      this.currentUser = null;
      this.userLoginSubject.next(false);
    } catch (err) {
      console.error('Error completing sign-out', err);
    }
  }

  async signinSilent(): Promise<User> {
    return await this.userManager.signinSilent();
  }

  async completeSilentSignIn(): Promise<void> {
    try {
      const user = await this.userManager.signinSilentCallback();
      this.currentUser = user;
      this.userLoginSubject.next(await this.isAuthenticated());
    } catch (err) {
      console.error('Silent sign-in callback failed', err);
      throw err;
    }
  }
}

export function getClientSettings(): UserManagerSettings {
  return {
    authority: environment.authority,
    client_id: environment.client_id,
    redirect_uri: environment.redirect_uri,
    silent_redirect_uri: environment.silent_redirect_uri,
    post_logout_redirect_uri: environment.post_logout_redirect_uri,
    response_type: environment.response_type,
    scope: environment.scope,
    filterProtocolClaims: true,
    loadUserInfo: true,
    automaticSilentRenew: true,
    userStore: new WebStorageStateStore({ store: window.localStorage }),
  };
}
