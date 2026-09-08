// core/auth/session-facade.ts
import { Injectable, inject } from '@angular/core';
import { AuthenticationService } from '../services/authentication/authentication-service';
import { UserService } from '../services/profile/user-service';
import { LoginRequest, RegisterRequest } from '../models/authentication-model';

@Injectable({ providedIn: 'root' })
export class SessionFacade {
  private readonly auth = inject(AuthenticationService);
  private readonly userService = inject(UserService);

  readonly isAuthenticated = this.auth.isAuthenticated;
  readonly userProfile = this.userService.userProfile;

  async login(payload: LoginRequest): Promise<void> {
    await this.auth.login(payload);
    await this.userService.loadUserProfile();
  }

  async loginWithGoogle(idToken: string): Promise<void> {
    await this.auth.loginWithGoogle({
      idToken,
    });

    await this.userService.loadUserProfile();
  }

  async refreshSession(): Promise<void> {
    await this.auth.refresh();
  }

  async register(payload: RegisterRequest): Promise<void> {
    await this.auth.register(payload);
    await this.userService.loadUserProfile();
  }

  async logout(): Promise<void> {
    await this.auth.logout();
    this.userService.clearProfile();
  }

  /** Called on app startup — restores the profile if a valid token already exists */
  async restoreSession(): Promise<void> {
    if (!this.auth.isAuthenticated()) {
      return;
    }

    await this.userService.loadUserProfile();
  }
}
