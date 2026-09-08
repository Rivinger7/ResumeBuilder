import {
  Component,
  computed,
  effect,
  inject,
  input,
  OnDestroy,
  output,
  signal,
} from '@angular/core';
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
  EducationEntryInternalResponse,
  ResumeSectionType,
  UpdateEducationEntryRequest,
} from '../../../../core/models/resume-model';
import { LucideIconModule } from '../../../../shared/utils/lucide-icon-module';
import { EducationCustomizationPanel } from './education-customization-panel/education-customization-panel';
import { EducationCustomization } from '../interfaces/customization/customization-model';

interface FormValue {
  title: string;
  degree: string;
  major: string;
  gpa: string;
  startDate: string;
  endDate: string;
  isCurrent: boolean;
  location: string;
  description: string;
}

const FIELDS: (keyof FormValue)[] = [
  'title',
  'degree',
  'major',
  'gpa',
  'isCurrent',
  'startDate',
  'endDate',
  'location',
  'description',
];

@Component({
  selector: 'app-education-entry-editor',
  imports: [
    ReactiveFormsModule,
    FormField,
    RichTextField,
    EntryEditorActions,
    LucideIconModule,
    EducationCustomizationPanel,
  ],
  templateUrl: './education-entry-editor.html',
})
export class EducationEntryEditor implements OnDestroy {
  private readonly formBuilder = inject(NonNullableFormBuilder);
  private readonly entryService = inject(EntryService);
  private readonly draftState = inject(DraftStateService);

  readonly isStyleCustomizationVisible = signal(false);
  readonly customization = signal<EducationCustomization>({
    isByOrder: false,
  });

  readonly resumeSectionId = input.required<string>();
  readonly entry = input<EducationEntryInternalResponse | null>(null);

  readonly saved = output<void>();
  readonly cancelled = output<void>();
  readonly deleted = output<void>();
  readonly changed = output<void>();

  readonly form = this.formBuilder.group({
    title: [''],
    degree: [''],
    major: [''],
    gpa: [''],
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
      title: entry.schoolName ?? '',
      degree: entry.degree ?? '',
      major: entry.major ?? '',
      gpa: entry.gpa != null ? String(entry.gpa) : '',
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
      const info = this.entry();
      this.customization.set({
        isByOrder: info?.isByOrder ?? false,
      });

      const patched = this.patchedValue();
      this.form.reset();
      this.originalValue = patched;
      if (patched) this.form.patchValue(patched);
    });

    // Push the draft to DraftStateService on every form change so ResumePreview renders
    // in real time without waiting for save().
    effect(() => {
      this.formValue(); // register dependency; value itself unused
      const v = this.form.getRawValue();
      this.draftState.set({
        sectionType: ResumeSectionType.Education,
        entryId: this.entry()?.id,
        values: {
          schoolName: v.title,
          degree: v.degree,
          major: v.major,
          gpa: v.gpa ? Number(v.gpa) : null,
          isCurrent: v.isCurrent,
          startDate: fromMonthInput(v.startDate),
          endDate: fromMonthInput(v.endDate),
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
        const payload = buildPartialPayload<FormValue, UpdateEducationEntryRequest>(
          current,
          this.originalValue,
          {
            schoolName: 'title',
            degree: 'degree',
            major: 'major',
            gpa: 'gpa',
            isCurrent: 'isCurrent',
            startDate: 'startDate',
            endDate: 'endDate',
            location: 'location',
            description: 'description',
          },
          {
            gpa: (raw) => (raw ? Number(raw) : null),
            startDate: (raw) => fromMonthInput(raw as string),
            endDate: (raw) => fromMonthInput(raw as string),
          },
        );
        await this.entryService.updateEducation(entryId, payload);
      }
    } else {
      await this.entryService.createEducation({
        resumeSectionId: this.resumeSectionId(),
        schoolName: current.title || null,
        degree: current.degree || null,
        major: current.major || null,
        gpa: current.gpa ? Number(current.gpa) : null,
        isCurrent: current.isCurrent,
        startDate: fromMonthInput(current.startDate),
        endDate: fromMonthInput(current.endDate),
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
    await this.entryService.deleteEducation(entryId);
    this.deleted.emit();
  }

  cancel(): void {
    this.draftState.clear();
    this.cancelled.emit();
  }

  toggleCustomization(): void {
    this.isStyleCustomizationVisible.set(!this.isStyleCustomizationVisible());
  }

  async onCustomizationChange(value: EducationCustomization): Promise<void> {
    this.customization.set(value);

    this.draftState.set({
      sectionType: ResumeSectionType.Education,
      entryId: this.entry()?.id,
      values: { ...this.form.getRawValue(), isByOrder: value.isByOrder },
    });

    const id = this.entry()?.id;
    if (!id) return;

    await this.entryService.updateEducationStyle(id, {
      isByOrder: value.isByOrder,
    });

    this.changed.emit();
  }
}
