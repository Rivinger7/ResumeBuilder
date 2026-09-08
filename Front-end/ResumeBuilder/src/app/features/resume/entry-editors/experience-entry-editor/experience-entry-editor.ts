import { Component, computed, effect, inject, input, OnDestroy, output } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { NonNullableFormBuilder, ReactiveFormsModule } from '@angular/forms';
import { FormField } from '../../../../shared/components/form-field/form-field';
import { RichTextField } from '../../../../shared/components/rich-text-field/rich-text-field';
import { EntryEditorActions } from '../../../../shared/components/entry-editor-actions/entry-editor-actions';
import { EntryService } from '../../../../core/services/resumes/entry-service';
import { DraftStateService } from '../../../../core/services/resumes/draft-state-service';
import { toEditorHtml } from '../../../../shared/utils/rich-text';
import {
  buildPartialPayload,
  fromMonthInput,
  hasFieldChanges,
  toMonthInput,
} from '../../../../shared/utils/entry-form-utils';
import {
  ExperienceEntryInternalResponse,
  ResumeSectionType,
  UpdateExperienceEntryRequest,
} from '../../../../core/models/resume-model';

interface FormValue {
  title: string;
  subTitle: string;
  startDate: string;
  endDate: string;
  isCurrent: boolean;
  location: string;
  description: string;
}

const FIELDS: (keyof FormValue)[] = [
  'subTitle',
  'title',
  'startDate',
  'endDate',
  'isCurrent',
  'location',
  'description',
];

@Component({
  selector: 'app-experience-entry-editor',
  imports: [ReactiveFormsModule, FormField, RichTextField, EntryEditorActions],
  templateUrl: './experience-entry-editor.html',
})
export class ExperienceEntryEditor implements OnDestroy {
  private readonly formBuilder = inject(NonNullableFormBuilder);
  private readonly entryService = inject(EntryService);
  private readonly draftState = inject(DraftStateService);

  readonly resumeSectionId = input.required<string>();
  readonly entry = input<ExperienceEntryInternalResponse | null>(null);

  readonly saved = output<void>();
  readonly cancelled = output<void>();
  readonly deleted = output<void>();

  readonly form = this.formBuilder.group({
    title: [''],
    subTitle: [''],
    startDate: [''],
    endDate: [''],
    isCurrent: [false],
    location: [''],
    description: [''],
  });

  private readonly patchedValue = computed<FormValue | null>(() => {
    const entry = this.entry();
    if (!entry) return null;
    return {
      title: entry.position ?? '',
      subTitle: entry.companyName ?? '',
      startDate: toMonthInput(entry.startDate),
      endDate: toMonthInput(entry.endDate),
      isCurrent: entry.isCurrent ?? false,
      location: entry.location ?? '',
      description: toEditorHtml(entry.description ?? ''),
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
        sectionType: ResumeSectionType.Experience,
        entryId: this.entry()?.id,
        values: {
          companyName: v.subTitle,
          position: v.title,
          startDate: fromMonthInput(v.startDate),
          endDate: fromMonthInput(v.endDate),
          isCurrent: v.isCurrent,
          location: v.location,
          description: v.description,
        },
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
        const payload = buildPartialPayload<FormValue, UpdateExperienceEntryRequest>(
          current,
          this.originalValue,
          {
            companyName: 'subTitle',
            position: 'title',
            startDate: 'startDate',
            endDate: 'endDate',
            isCurrent: 'isCurrent',
            location: 'location',
            description: 'description',
          },
          {
            startDate: (raw) => fromMonthInput(raw as string),
            endDate: (raw) => fromMonthInput(raw as string),
          },
        );
        await this.entryService.updateExperience(entryId, payload);
      }
    } else {
      await this.entryService.createExperience({
        resumeSectionId: this.resumeSectionId(),
        companyName: current.subTitle || null,
        position: current.title || null,
        startDate: fromMonthInput(current.startDate),
        endDate: fromMonthInput(current.endDate),
        isCurrent: current.isCurrent,
        location: current.location || null,
        description: current.description || null,
        isByOrder: false,
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
    await this.entryService.deleteExperience(entryId);
    this.deleted.emit();
  }

  cancel(): void {
    this.draftState.clear();
    this.cancelled.emit();
  }
}
