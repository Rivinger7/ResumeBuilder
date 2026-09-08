import { Injectable } from '@angular/core';

/** Thin wrapper around localStorage so ApiClientService isn't hard-coupled to it. */
@Injectable({ providedIn: 'root' })
export class AuthTokenService {
  private readonly key = 'accessToken';

  get(): string | null {
    return localStorage.getItem(this.key);
  }

  set(token: string): void {
    localStorage.setItem(this.key, token);
  }

  clear(): void {
    localStorage.removeItem(this.key);
  }
}
