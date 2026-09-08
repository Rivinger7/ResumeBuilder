import { Component, ElementRef, inject, signal, ViewChild } from '@angular/core';
import { DialogRef } from '@angular/cdk/dialog';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { DIALOG_DATA } from '@angular/cdk/dialog';
import { HlmFieldImports } from '@spartan-ng/helm/field';
import { HlmInputImports } from '@spartan-ng/helm/input';
import { HlmButtonImports } from '@spartan-ng/helm/button';
import { LucideIconModule } from '../../../shared/utils/lucide-icon-module';
import { SessionFacade } from '../../../core/facade/session-facade';
import { passwordMatchValidator } from '../../../shared/validators/password-match-validator';
import { AuthenticationGoogleService } from '../../../core/services/authentication/authentication-google-service';

type AuthMode = 'login' | 'register';

@Component({
  selector: 'app-auth-dialog',
  imports: [
    ReactiveFormsModule,
    HlmInputImports,
    HlmFieldImports,
    HlmButtonImports,
    LucideIconModule,
  ],
  templateUrl: './auth-dialog.html',
})
export class AuthDialog {
  private formBuilder = inject(NonNullableFormBuilder);
  private dialogRef = inject(DialogRef);
  private session = inject(SessionFacade);
  private googleAuth = inject(AuthenticationGoogleService);
  private data = inject(DIALOG_DATA, { optional: true });

  loginSubmitted = signal(false);
  registerSubmitted = signal(false);
  loginError = signal<string | null>(null);

  mode = signal<AuthMode>(this.data?.mode ?? 'login');

  loginForm = this.formBuilder.group(
    {
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    },
    { updateOn: 'submit' },
  );

  registerForm = this.formBuilder.group(
    {
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      confirmedPassword: ['', Validators.required],
      fullName: ['', Validators.required],
    },
    {
      validators: passwordMatchValidator,
      updateOn: 'submit',
    },
  );

  switchTo(mode: AuthMode) {
    this.mode.set(mode);
  }

  async submitLogin() {
    this.loginSubmitted.set(true);
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.loginError.set(null);

    try {
      const payload = this.loginForm.getRawValue();
      await this.session.login(payload);

      this.dialogRef.close();
      this.loginForm.reset();
      this.loginSubmitted.set(true);
    } catch {
      this.loginError.set('Email or password is invalid.');
    }
  }

  @ViewChild('googleButton')
  set googleButton(element: ElementRef<HTMLDivElement> | undefined) {
    if (!element) {
      return;
    }

    queueMicrotask(() => {
      this.googleAuth.initialize(element.nativeElement, async (credential) => {
        try {
          await this.session.loginWithGoogle(credential);
          this.dialogRef.close();
        } catch (error) {
          console.error('Google login failed:', error);
        }
      });
    });
  }

  async submitRegister() {
    this.registerSubmitted.set(true);
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    try {
      const { confirmedPassword, ...payload } = this.registerForm.getRawValue();
      await this.session.register(payload);

      this.dialogRef.close();
      this.registerForm.reset();
      this.registerSubmitted.set(false);
    } catch {
      this.loginError.set('Unable to create account.');
    }
  }

  close() {
    this.dialogRef.close();
  }
}
