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
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { LucideIconModule } from '../../../../shared/utils/lucide-icon-module';
import { RichTextField } from '../../../../shared/components/rich-text-field/rich-text-field';
import { EntryEditorActions } from '../../../../shared/components/entry-editor-actions/entry-editor-actions';
import { EntryService } from '../../../../core/services/resumes/entry-service';
import { DraftStateService } from '../../../../core/services/resumes/draft-state-service';
import { toEditorHtml } from '../../../../shared/utils/rich-text';
import { buildPartialPayload, hasFieldChanges } from '../../../../shared/utils/entry-form-utils';
import {
  CertificateEntryInternalResponse,
  LayoutType,
  ResumeSectionType,
  UpdateCertificateEntryRequest,
} from '../../../../core/models/resume-model';
import { CertificationCustomizationPanel } from './certification-customization-panel/certification-customization-panel';
import { CertificationCustomization } from '../interfaces/customization/customization-model';

interface FormValue {
  title: string;
  linkUrl: string;
  description: string;
}

const FIELDS: (keyof FormValue)[] = ['title', 'linkUrl', 'description'];

@Component({
  selector: 'app-certificate-entry-editor',
  imports: [
    ReactiveFormsModule,
    LucideIconModule,
    RichTextField,
    EntryEditorActions,
    CertificationCustomizationPanel,
  ],
  templateUrl: './certificate-entry-editor.html',
})
export class CertificateEntryEditor implements OnDestroy {
  private readonly formBuilder = inject(NonNullableFormBuilder);
  private readonly entryService = inject(EntryService);
  private readonly draftState = inject(DraftStateService);

  readonly isStyleCustomizationVisible = signal(false);
  readonly customization = signal<CertificationCustomization>({
    certificateLayout: LayoutType.Grid,
    certificateGridColumn: 2,
    certificateRowSpacing: null,
    isStartRowsWithBullet: false,
    subinfoStyle: 'Default',
  });

  readonly resumeSectionId = input.required<string>();
  readonly entry = input<CertificateEntryInternalResponse | null>(null);

  readonly saved = output<void>();
  readonly cancelled = output<void>();
  readonly deleted = output<void>();

  readonly form = this.formBuilder.group({
    title: [''],
    linkUrl: ['', [Validators.pattern(/^https?:\/\/\S+$/i)]],
    description: [''],
  });

  private readonly patchedValue = computed<FormValue | null>(() => {
    const entry = this.entry();
    if (!entry) return null;
    return {
      title: entry.title ?? '',
      linkUrl: entry.certificateUrl ?? '',
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
        certificateLayout: info?.certificateLayout ?? LayoutType.Grid,
        certificateGridColumn: info?.certificateGridColumn ?? 2,
        certificateRowSpacing: info?.certificateRowSpacing ?? 'Tight',
        isStartRowsWithBullet: info?.isStartRowsWithBullet ?? false,
        subinfoStyle: info?.subinfoStyle ?? 'Default',
      });

      const patched = this.patchedValue();
      this.form.reset();
      this.originalValue = patched;
      if (patched) this.form.patchValue(patched);
    });

    effect(() => {
      this.formValue();
      const v = this.form.getRawValue();
      this.draftState.set({
        sectionType: ResumeSectionType.Certificates,
        entryId: this.entry()?.id,
        values: {
          title: v.title,
          certificateUrl: v.linkUrl,
          description: v.description,
          certificateLayout: this.customization().certificateLayout,
          certificateGridColumn: this.customization().certificateGridColumn,
          certificateRowSpacing: this.customization().certificateRowSpacing,
          isStartRowsWithBullet: this.customization().isStartRowsWithBullet,
          subinfoStyle: this.customization().subinfoStyle,
        },
      });
    });
  }

  ngOnDestroy(): void {
    this.draftState.clear();
  }

  async save(): Promise<void> {
    if (this.form.invalid) {
      return;
    }

    const entryId = this.entry()?.id;
    const current = this.form.getRawValue();

    if (entryId) {
      if (hasFieldChanges(current, this.originalValue, FIELDS)) {
        const payload = buildPartialPayload<FormValue, UpdateCertificateEntryRequest>(
          current,
          this.originalValue,
          { title: 'title', certificateUrl: 'linkUrl', description: 'description' },
        );
        await this.entryService.updateCertificate(entryId, payload);
      }
    } else {
      await this.entryService.createCertificate({
        resumeSectionId: this.resumeSectionId(),
        title: current.title || null,
        certificateUrl: current.linkUrl || null,
        description: current.description || null,
        certificateLayout: LayoutType.Grid,
        certificateGridColumn: 2,
        certificateRowSpacing: null,
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
    await this.entryService.deleteCertificate(entryId);
    this.deleted.emit();
  }

  cancel(): void {
    this.draftState.clear();
    this.cancelled.emit();
  }

  toggleCustomization(): void {
    this.isStyleCustomizationVisible.set(!this.isStyleCustomizationVisible());
  }

  async onCustomizationChange(value: CertificationCustomization): Promise<void> {
    const previous = this.customization();
    this.customization.set(value);

    const current = this.form.getRawValue();

    this.draftState.set({
      sectionType: ResumeSectionType.Certificates,
      entryId: this.entry()?.id,
      values: {
        title: current.title,
        certificateUrl: current.linkUrl,
        description: current.description,
        certificateLayout: value.certificateLayout,
        certificateGridColumn: value.certificateGridColumn,
        certificateRowSpacing: value.certificateRowSpacing,
        isStartRowsWithBullet: value.isStartRowsWithBullet,
        subinfoStyle: value.subinfoStyle,
      },
    });

    const entryId = this.entry()?.id;
    if (!entryId) return;

    if (
      value.certificateLayout === previous.certificateLayout &&
      value.certificateGridColumn === previous.certificateGridColumn &&
      value.certificateRowSpacing === previous.certificateRowSpacing &&
      value.isStartRowsWithBullet === previous.isStartRowsWithBullet &&
      value.subinfoStyle === previous.subinfoStyle
    ) {
      return;
    }

    await this.entryService.updateCertificateStyle(entryId, {
      certificateLayout: value.certificateLayout,
      certificateGridColumn:
        value.certificateLayout === LayoutType.Rows ? null : value.certificateGridColumn,
      certificateRowSpacing:
        value.certificateLayout === LayoutType.Rows
          ? (value.certificateRowSpacing ?? 'Tight')
          : null,
      isStartRowsWithBullet: value.isStartRowsWithBullet,
      subinfoStyle: value.subinfoStyle ?? 'Default',
    });
  }
}
