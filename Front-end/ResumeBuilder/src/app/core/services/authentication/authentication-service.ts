import { Injectable, computed, inject, signal } from '@angular/core';
import { ApiClientService } from '../../services/api-client-service';
import { GlobalErrorHandlerService } from '../../services/global-error-handler-service';
import {
  LoginRequest,
  RegisterRequest,
  LoginWithGoogleRequest,
  LoginResponse,
  RefreshTokenResponse,
  RegisterResponse,
} from '../../models/authentication-model';
import { AuthTokenService } from '../auth-token-service';

const ACCESS_TOKEN_KEY = 'accessToken';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
  private readonly api = inject(ApiClientService);
  private readonly errorHandler = inject(GlobalErrorHandlerService);
  private readonly tokenStore = inject(AuthTokenService);

  // signal so components can react to auth state
  private _accessToken = signal<string | null>(this.tokenStore.get());
  readonly accessToken = this._accessToken.asReadonly();
  readonly isAuthenticated = computed(() => this._accessToken() !== null);
  private readonly url = '/auth';

  async register(request: RegisterRequest): Promise<void> {
    try {
      const data = await this.api.post<RegisterResponse>(`${this.url}/register`, request, {
        withCredentials: true,
      });
      this.setToken(data);
    } catch (error) {
      this.errorHandler.handle(error);
      throw error;
    }
  }

  async login(request: LoginRequest): Promise<void> {
    try {
      const data = await this.api.post<LoginResponse>(`${this.url}/login`, request, {
        withCredentials: true,
      });
      this.setToken(data);
    } catch (error) {
      this.errorHandler.handle(error);
      throw error;
    }
  }

  async loginWithGoogle(request: LoginWithGoogleRequest): Promise<void> {
    try {
      const data = await this.api.post<LoginResponse>(`${this.url}/login-with-google`, request, {
        withCredentials: true,
      });
      this.setToken(data);
    } catch (error) {
      this.errorHandler.handle(error);
      throw error;
    }
  }

  async refresh(): Promise<void> {
    // refreshToken lives in a cookie, BE reads it from Request.Cookies -> no body needed
    try {
      const data = await this.api.post<RefreshTokenResponse>(`${this.url}/refresh`, undefined, {
        withCredentials: true,
      });
      this.setToken(data);
    } catch (error) {
      this.errorHandler.handle(error);
      throw error;
    }
  }

  async logout(): Promise<void> {
    try {
      await this.api.post<void>(`${this.url}/logout`, undefined, { withCredentials: true });
    } catch {
      // Ignore logout API errors (e.g. expired token) — still clear the local token
      // so the caller isn't blocked.
    } finally {
      this.clearToken();
    }
  }

  private setToken(token: string): void {
    this.tokenStore.set(token);
    this._accessToken.set(token);
  }

  private clearToken(): void {
    this.tokenStore.clear();
    this._accessToken.set(null);
  }
}
