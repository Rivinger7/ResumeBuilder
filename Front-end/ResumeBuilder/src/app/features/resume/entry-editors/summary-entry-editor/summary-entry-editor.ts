import { Component, computed, effect, inject, input, OnDestroy, output } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { NonNullableFormBuilder, ReactiveFormsModule } from '@angular/forms';
import { RichTextField } from '../../../../shared/components/rich-text-field/rich-text-field';
import { EntryEditorActions } from '../../../../shared/components/entry-editor-actions/entry-editor-actions';
import { EntryService } from '../../../../core/services/resumes/entry-service';
import { DraftStateService } from '../../../../core/services/resumes/draft-state-service';
import { toEditorHtml } from '../../../../shared/utils/rich-text';
import { isEqualFormValue } from '../../../../shared/utils/entry-form-utils';
import { ResumeSectionType, SummaryEntryInternalResponse } from '../../../../core/models/resume-model';

@Component({
  selector: 'app-summary-entry-editor',
  imports: [ReactiveFormsModule, RichTextField, EntryEditorActions],
  templateUrl: './summary-entry-editor.html',
})
export class SummaryEntryEditor implements OnDestroy {
  private readonly formBuilder = inject(NonNullableFormBuilder);
  private readonly entryService = inject(EntryService);
  private readonly draftState = inject(DraftStateService);

  readonly resumeSectionId = input.required<string>();
  readonly entry = input<SummaryEntryInternalResponse | null>(null);

  readonly saved = output<void>();
  readonly cancelled = output<void>();
  readonly deleted = output<void>();

  readonly form = this.formBuilder.group({
    description: [''],
  });

  private readonly patchedValue = computed<string | null>(() => {
    const entry = this.entry();
    return entry ? toEditorHtml(entry.summary ?? '') : null;
  });

  private originalValue: string | null = null;

  private readonly formValue = toSignal(this.form.valueChanges, {
    initialValue: this.form.getRawValue(),
  });

  constructor() {
    effect(() => {
      const patched = this.patchedValue();
      this.form.reset();
      this.originalValue = patched;
      if (patched) this.form.patchValue({ description: patched });
    });

    effect(() => {
      this.formValue();
      const v = this.form.getRawValue();
      this.draftState.set({
        sectionType: ResumeSectionType.Summary,
        entryId: this.entry()?.id,
        values: { summary: v.description },
      });
    });
  }

  ngOnDestroy(): void {
    this.draftState.clear();
  }

  async save(): Promise<void> {
    const entryId = this.entry()?.id;
    const current = this.form.getRawValue();

    if (entryId) {
      if (!isEqualFormValue(current.description, this.originalValue)) {
        await this.entryService.updateSummary(entryId, { summary: current.description || null });
      }
    } else {
      await this.entryService.createSummary({
        resumeSectionId: this.resumeSectionId(),
        summary: current.description || null,
      });
    }

    this.draftState.clear();
    this.saved.emit();
  }

  async remove(): Promise<void> {
    const entryId = this.entry()?.id;
    if (!entryId) {
      this.cancelled.emit();
      return;
    }
    await this.entryService.deleteSummary(entryId);
    this.deleted.emit();
  }

  cancel(): void {
    this.draftState.clear();
    this.cancelled.emit();
  }
}
