import { Component, inject, signal } from '@angular/core';
import { DialogRef } from '@angular/cdk/dialog';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { HlmFieldImports } from '@spartan-ng/helm/field';
import { HlmInputImports } from '@spartan-ng/helm/input';
import { HlmButtonImports } from '@spartan-ng/helm/button';
import { ResumeService } from '../../../core/services/resumes/resume-service';
import { CreateResumeResponse } from '../../../core/models/resume-model';
import { ApiError } from '../../../core/errors/api-error';

@Component({
  selector: 'app-create-resume-dialog',
  imports: [ReactiveFormsModule, HlmInputImports, HlmFieldImports, HlmButtonImports],
  templateUrl: './create-resume-dialog.html',
})
export class CreateResumeDialog {
  private readonly formBuilder = inject(NonNullableFormBuilder);
  private readonly dialogRef = inject(DialogRef<CreateResumeResponse | undefined>);
  private readonly resumeService = inject(ResumeService);

  readonly isSubmitting = signal(false);
  readonly submitError = signal<string | null>(null);

  form = this.formBuilder.group({
    title: ['', [Validators.required, Validators.maxLength(200)]],
    description: [''],
  });

  async submit(): Promise<void> {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.submitError.set(null);
    this.isSubmitting.set(true);
    try {
      const { title, description } = this.form.getRawValue();
      const response = await this.resumeService.create(
        { title, description: description || null },
        { silent: true },
      );
      this.dialogRef.close(response);
    } catch (error) {
      this.submitError.set(
        error instanceof ApiError ? error.message : 'Could not create resume.',
      );
    } finally {
      this.isSubmitting.set(false);
    }
  }

  close(): void {
    this.dialogRef.close(undefined);
  }
}
