import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Sidebar } from './core/layout/user-layout/sidebar/sidebar';
import { HlmSidebarImports } from '@spartan-ng/helm/sidebar';
import { HlmSelectImports } from '@spartan-ng/helm/select';
import { HlmButtonImports } from '@spartan-ng/helm/button';
import { HlmButtonGroupImports } from '@spartan-ng/helm/button-group';
import { AuthenticationService } from './core/services/authentication/authentication-service';
import { AuthDialogService } from './features/authentication/auth-dialog/auth-dialog-service';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    Sidebar,
    HlmSidebarImports,
    HlmSelectImports,
    HlmButtonImports,
    HlmButtonGroupImports,
  ],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  protected readonly title = signal('ResumeBuilder');

  private readonly authService = inject(AuthenticationService);
  private readonly authDialog = inject(AuthDialogService);

  readonly isAuthenticated = this.authService.isAuthenticated;

  languages = [
    { label: 'English', value: 'en' },
    { label: 'Vietnamese', value: 'vn' },
  ];

  itemToString = (value: string) => this.languages.find((x) => x.value === value)?.label ?? '';

  openLoginDialog() {
    this.authDialog.openLoginDialog();
  }

  openRegisterDialog() {
    this.authDialog.openRegisterDialog();
  }
}
