import { Component, computed, effect, inject, input, OnDestroy, output } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { NonNullableFormBuilder, ReactiveFormsModule } from '@angular/forms';
import { LucideIconModule } from '../../../../shared/utils/lucide-icon-module';
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
  ProjectEntryInternalResponse,
  ResumeSectionType,
  UpdateProjectEntryRequest,
} from '../../../../core/models/resume-model';

interface FormValue {
  title: string;
  subTitle: string;
  startDate: string;
  endDate: string;
  linkUrl: string;
  description: string;
}

const FIELDS: (keyof FormValue)[] = ['title', 'subTitle', 'startDate', 'endDate', 'linkUrl', 'description'];

@Component({
  selector: 'app-project-entry-editor',
  imports: [ReactiveFormsModule, LucideIconModule, FormField, RichTextField, EntryEditorActions],
  templateUrl: './project-entry-editor.html',
})
export class ProjectEntryEditor implements OnDestroy {
  private readonly formBuilder = inject(NonNullableFormBuilder);
  private readonly entryService = inject(EntryService);
  private readonly draftState = inject(DraftStateService);

  readonly resumeSectionId = input.required<string>();
  readonly entry = input<ProjectEntryInternalResponse | null>(null);

  readonly saved = output<void>();
  readonly cancelled = output<void>();
  readonly deleted = output<void>();

  readonly form = this.formBuilder.group({
    title: [''],
    subTitle: [''],
    startDate: [''],
    endDate: [''],
    linkUrl: [''],
    description: [''],
  });

  private readonly patchedValue = computed<FormValue | null>(() => {
    const entry = this.entry();
    if (!entry) return null;
    return {
      title: entry.title ?? '',
      subTitle: entry.subTitle ?? '',
      startDate: toMonthInput(entry.startDate),
      endDate: toMonthInput(entry.endDate),
      linkUrl: entry.projectUrl ?? '',
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
        sectionType: ResumeSectionType.Projects,
        entryId: this.entry()?.id,
        values: {
          title: v.title,
          subTitle: v.subTitle,
          startDate: fromMonthInput(v.startDate),
          endDate: fromMonthInput(v.endDate),
          projectUrl: v.linkUrl,
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
        const payload = buildPartialPayload<FormValue, Omit<UpdateProjectEntryRequest, 'repositoryUrl'>>(
          current,
          this.originalValue,
          {
            title: 'title',
            subTitle: 'subTitle',
            startDate: 'startDate',
            endDate: 'endDate',
            projectUrl: 'linkUrl',
            description: 'description',
          },
          {
            startDate: (raw) => fromMonthInput(raw as string),
            endDate: (raw) => fromMonthInput(raw as string),
          },
        );
        // repositoryUrl: no form input yet — always send null (keep existing BE value).
        await this.entryService.updateProject(entryId, { ...payload, repositoryUrl: null });
      }
    } else {
      await this.entryService.createProject({
        resumeSectionId: this.resumeSectionId(),
        title: current.title || null,
        subTitle: current.subTitle || null,
        startDate: fromMonthInput(current.startDate),
        endDate: fromMonthInput(current.endDate),
        projectUrl: current.linkUrl || null,
        repositoryUrl: null,
        description: current.description || null,
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
    await this.entryService.deleteProject(entryId);
    this.deleted.emit();
  }

  cancel(): void {
    this.draftState.clear();
    this.cancelled.emit();
  }
}
