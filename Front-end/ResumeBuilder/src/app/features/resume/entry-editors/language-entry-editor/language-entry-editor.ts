import { Component, computed, effect, inject, input, OnDestroy, output } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { NonNullableFormBuilder, ReactiveFormsModule } from '@angular/forms';
import { FormField } from '../../../../shared/components/form-field/form-field';
import { EntryEditorActions } from '../../../../shared/components/entry-editor-actions/entry-editor-actions';
import { EntryService } from '../../../../core/services/resumes/entry-service';
import { DraftStateService } from '../../../../core/services/resumes/draft-state-service';
import { buildPartialPayload, hasFieldChanges } from '../../../../shared/utils/entry-form-utils';
import {
  LanguageEntryInternalResponse,
  LayoutType,
  ResumeSectionType,
  UpdateLanguageEntryRequest,
} from '../../../../core/models/resume-model';

interface FormValue {
  title: string;
  proficiency: string;
}

const FIELDS: (keyof FormValue)[] = ['title', 'proficiency'];

@Component({
  selector: 'app-language-entry-editor',
  imports: [ReactiveFormsModule, FormField, EntryEditorActions],
  templateUrl: './language-entry-editor.html',
})
export class LanguageEntryEditor implements OnDestroy {
  private readonly formBuilder = inject(NonNullableFormBuilder);
  private readonly entryService = inject(EntryService);
  private readonly draftState = inject(DraftStateService);

  readonly resumeSectionId = input.required<string>();
  readonly entry = input<LanguageEntryInternalResponse | null>(null);

  readonly saved = output<void>();
  readonly cancelled = output<void>();
  readonly deleted = output<void>();

  readonly form = this.formBuilder.group({
    title: [''],
    proficiency: [''],
  });

  private readonly patchedValue = computed<FormValue | null>(() => {
    const entry = this.entry();
    if (!entry) return null;
    return {
      title: entry.languageName ?? '',
      proficiency: entry.proficiency ?? '',
    };
  });

  private originalValue: FormValue | null = null;

  private readonly formValue = toSignal(this.form.valueChanges, {
    initialValue: this.form.getRawValue(),
  });

  constructor() {
    effect(() => {
      const patched = this.patchedValue();
      this.form.reset();
      this.originalValue = patched;
      if (patched) this.form.patchValue(patched);
    });

    effect(() => {
      this.formValue();
      const v = this.form.getRawValue();
      this.draftState.set({
        sectionType: ResumeSectionType.Languages,
        entryId: this.entry()?.id,
        values: { languageName: v.title, proficiency: v.proficiency },
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
      if (hasFieldChanges(current, this.originalValue, FIELDS)) {
        const payload = buildPartialPayload<FormValue, UpdateLanguageEntryRequest>(
          current,
          this.originalValue,
          { languageName: 'title', proficiency: 'proficiency' },
        );
        await this.entryService.updateLanguage(entryId, payload);
      }
    } else {
      await this.entryService.createLanguage({
        resumeSectionId: this.resumeSectionId(),
        languageName: current.title || null,
        proficiency: current.proficiency || null,
        languageLayout: LayoutType.Compact,
        languageGridColumn: null,
        languageRowSpacing: null,
        isStartRowsWithBullet: false,
        subinfoStyle: null,
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
    await this.entryService.deleteLanguage(entryId);
    this.deleted.emit();
  }

  cancel(): void {
    this.draftState.clear();
    this.cancelled.emit();
  }
}
