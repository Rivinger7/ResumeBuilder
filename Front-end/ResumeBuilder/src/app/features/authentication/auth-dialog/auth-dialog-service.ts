import { Dialog } from '@angular/cdk/dialog';
import { Injectable, inject } from '@angular/core';
import { AuthDialog } from './auth-dialog';

@Injectable({ providedIn: 'root' })
export class AuthDialogService {
  private readonly dialog = inject(Dialog);

  openLoginDialog() {
    this.dialog.open(AuthDialog, {
      data: { mode: 'login' },
      hasBackdrop: true,
      backdropClass: ['backdrop-blur-xs', 'bg-black/30'],
    });
  }

  openRegisterDialog() {
    this.dialog.open(AuthDialog, {
      data: { mode: 'register' },
      hasBackdrop: true,
      backdropClass: ['backdrop-blur-xs', 'bg-black/30'],
    });
  }
}
