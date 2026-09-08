import { Injectable } from '@angular/core';

declare const google: any;

@Injectable({
  providedIn: 'root',
})
export class AuthenticationGoogleService {
  initialize(buttonElement: HTMLElement, onCredential: (credential: string) => void): void {
    const clientId = import.meta.env['NG_APP_GOOGLE_CLIENT_ID'];

    if (!clientId) {
      throw new Error('NG_APP_GOOGLE_CLIENT_ID is not configured.');
    }

    if (typeof google === 'undefined' || !google.accounts?.id) {
      throw new Error('Google Identity Services SDK is not loaded.');
    }

    google.accounts.id.initialize({
      client_id: clientId,

      auto_select: false,
      ux_mode: 'popup',

      callback: (response: { credential?: string }) => {
        if (!response.credential) {
          console.error('Google did not return a credential.');
          return;
        }

        onCredential(response.credential);
      },
    });

    google.accounts.id.renderButton(buttonElement, {
      type: 'standard',
      theme: 'outline',
      size: 'large',
      text: 'signin_with',
      shape: 'rectangular',
      width: 320,
    });
  }

  logout(): void {
    google.accounts.id.disableAutoSelect();
  }
}
