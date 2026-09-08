import { Component, computed, effect, inject, input, OnDestroy, output } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { NonNullableFormBuilder, ReactiveFormsModule } from '@angular/forms';
import { FormField } from '../../../../shared/components/form-field/form-field';
import { RichTextField } from '../../../../shared/components/rich-text-field/rich-text-field';
import { EntryEditorActions } from '../../../../shared/components/entry-editor-actions/entry-editor-actions';
import { EntryService } from '../../../../core/services/resumes/entry-service';
import { DraftStateService } from '../../../../core/services/resumes/draft-state-service';
import { toEditorHtml } from '../../../../shared/utils/rich-text';
import { buildPartialPayload, hasFieldChanges } from '../../../../shared/utils/entry-form-utils';
import {
  LayoutType,
  ResumeSectionType,
  SkillEntryInternalResponse,
  UpdateSkillEntryRequest,
} from '../../../../core/models/resume-model';

interface FormValue {
  title: string;
  skillLevel: string;
  description: string;
}

const FIELDS: (keyof FormValue)[] = ['title', 'description', 'skillLevel'];

@Component({
  selector: 'app-skill-entry-editor',
  imports: [ReactiveFormsModule, FormField, RichTextField, EntryEditorActions],
  templateUrl: './skill-entry-editor.html',
})
export class SkillEntryEditor implements OnDestroy {
  private readonly formBuilder = inject(NonNullableFormBuilder);
  private readonly entryService = inject(EntryService);
  private readonly draftState = inject(DraftStateService);

  readonly resumeSectionId = input.required<string>();
  readonly entry = input<SkillEntryInternalResponse | null>(null);

  readonly saved = output<void>();
  readonly cancelled = output<void>();
  readonly deleted = output<void>();

  readonly form = this.formBuilder.group({
    title: [''],
    skillLevel: [''],
    description: [''],
  });

  private readonly patchedValue = computed<FormValue | null>(() => {
    const entry = this.entry();
    if (!entry) return null;
    return {
      title: entry.skillName ?? '',
      skillLevel: entry.skillLevel ?? '',
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
        sectionType: ResumeSectionType.Skills,
        entryId: this.entry()?.id,
        values: { skillName: v.title, description: v.description, skillLevel: v.skillLevel },
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
        const payload = buildPartialPayload<FormValue, UpdateSkillEntryRequest>(
          current,
          this.originalValue,
          { skillName: 'title', description: 'description', skillLevel: 'skillLevel' },
        );
        await this.entryService.updateSkill(entryId, payload);
      }
    } else {
      await this.entryService.createSkill({
        resumeSectionId: this.resumeSectionId(),
        skillName: current.title || null,
        description: current.description || null,
        skillLevel: current.skillLevel || null,
        skillLayout: LayoutType.Compact,
        skillGridColumn: null,
        skillRowSpacing: null,
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
    await this.entryService.deleteSkill(entryId);
    this.deleted.emit();
  }

  cancel(): void {
    this.draftState.clear();
    this.cancelled.emit();
  }
}
