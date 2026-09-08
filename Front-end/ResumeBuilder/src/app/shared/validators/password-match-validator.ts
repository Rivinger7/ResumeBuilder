import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export const passwordMatchValidator: ValidatorFn = (
  control: AbstractControl,
): ValidationErrors | null => {
  const password = control.get('password')?.value;
  const confirmedPassword = control.get('confirmedPassword')?.value;

  if (password !== confirmedPassword) {
    return { passwordMismatch: true };
  }

  return null;
};
