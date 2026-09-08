import { Component, computed, effect, inject, input, OnDestroy, output, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { NonNullableFormBuilder, ReactiveFormsModule } from '@angular/forms';
import { LucideIconModule } from '../../../../shared/utils/lucide-icon-module';
import { FormField } from '../../../../shared/components/form-field/form-field';
import { EntryEditorActions } from '../../../../shared/components/entry-editor-actions/entry-editor-actions';
import { EntryService } from '../../../../core/services/resumes/entry-service';
import { DraftStateService } from '../../../../core/services/resumes/draft-state-service';
import { ApiClientService } from '../../../../core/services/api-client-service';
import { buildPartialPayload, hasFieldChanges, isEqualFormValue } from '../../../../shared/utils/entry-form-utils';
import {
  PersonalInformationEntryInternalResponse,
  ResumeSectionType,
  UpdatePersonalInformationEntryRequest,
} from '../../../../core/models/resume-model';

interface FormValue {
  title: string;
  subTitle: string;
  email: string;
  phoneNumber: string;
  address: string;
  website: string;
  linkedin: string;
  github: string;
}

const FIELDS: (keyof FormValue)[] = [
  'title',
  'subTitle',
  'email',
  'phoneNumber',
  'address',
  'website',
  'linkedin',
  'github',
];

@Component({
  selector: 'app-personal-information-entry-editor',
  imports: [ReactiveFormsModule, LucideIconModule, FormField, EntryEditorActions],
  templateUrl: './personal-information-entry-editor.html',
})
export class PersonalInformationEntryEditor implements OnDestroy {
  private readonly formBuilder = inject(NonNullableFormBuilder);
  private readonly entryService = inject(EntryService);
  private readonly draftState = inject(DraftStateService);
  private readonly apiClient = inject(ApiClientService);

  readonly resumeSectionId = input.required<string>();
  readonly entry = input<PersonalInformationEntryInternalResponse | null>(null);

  readonly saved = output<void>();
  readonly cancelled = output<void>();

  // Photo upload — uploads immediately on file selection so the preview updates right
  // away; the resulting URL is only persisted to the entry on save().
  readonly isUploadingPhoto = signal(false);
  private readonly uploadedPhotoUrl = signal<string | null>(null);

  readonly photoPreviewUrl = computed<string | null>(() => {
    const uploaded = this.uploadedPhotoUrl();
    if (uploaded) return `${this.apiClient.origin}${uploaded}`;

    const existing = this.entry()?.photoUrl;
    return existing ? `${this.apiClient.origin}${existing}` : null;
  });

  readonly form = this.formBuilder.group({
    title: [''],
    subTitle: [''],
    email: [''],
    phoneNumber: [''],
    address: [''],
    website: [''],
    linkedin: [''],
    github: [''],
  });

  private readonly patchedValue = computed<FormValue | null>(() => {
    const entry = this.entry();
    if (!entry) return null;
    return {
      title: entry.fullName ?? '',
      subTitle: entry.professionalTitle ?? '',
      email: entry.email ?? '',
      phoneNumber: entry.phoneNumber ?? '',
      address: entry.address ?? '',
      website: entry.website ?? '',
      linkedin: entry.linkedin ?? '',
      github: entry.github ?? '',
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
      this.uploadedPhotoUrl.set(null);
      if (patched) this.form.patchValue(patched);
    });

    effect(() => {
      this.formValue();
      const v = this.form.getRawValue();
      this.draftState.set({
        sectionType: ResumeSectionType.PersonalInformation,
        entryId: this.entry()?.id,
        values: {
          fullName: v.title,
          professionalTitle: v.subTitle,
          email: v.email,
          phoneNumber: v.phoneNumber,
          address: v.address,
          website: v.website,
          linkedin: v.linkedin,
          github: v.github,
        },
      });
    });
  }

  ngOnDestroy(): void {
    this.draftState.clear();
  }

  async onPhotoSelected(event: Event): Promise<void> {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    input.value = ''; // allow re-selecting the same file later
    if (!file) return;

    const entryId = this.entry()?.id;
    if (!entryId) return;

    this.isUploadingPhoto.set(true);
    try {
      const response = await this.entryService.uploadPersonalInformationPhoto(entryId, file);
      this.uploadedPhotoUrl.set(response.photoUrl);
    } finally {
      this.isUploadingPhoto.set(false);
    }
  }

  async save(): Promise<void> {
    // PersonalInformation always exists already (created with the resume) — nothing to do
    // in create mode.
    const entryId = this.entry()?.id;
    if (!entryId) return;

    if (hasFieldChanges(this.form.getRawValue(), this.originalValue, FIELDS)) {
      const current = this.form.getRawValue();
      const original = this.originalValue;

      const payload = buildPartialPayload<
        FormValue,
        Omit<UpdatePersonalInformationEntryRequest, 'fullName' | 'professionalTitle' | 'photoUrl'>
      >(current, original, {
        email: 'email',
        phoneNumber: 'phoneNumber',
        address: 'address',
        website: 'website',
        linkedIn: 'linkedin',
        gitHub: 'github',
      });

      // fullName/professionalTitle are non-nullable on the BE — resend the original
      // value when unchanged instead of null.
      await this.entryService.updatePersonalInformation(entryId, {
        ...payload,
        fullName: isEqualFormValue(current.title, original?.title) ? original!.title : current.title,
        professionalTitle: isEqualFormValue(current.subTitle, original?.subTitle)
          ? original!.subTitle
          : current.subTitle,
        photoUrl: this.uploadedPhotoUrl() ?? this.entry()?.photoUrl ?? null,
      });
    }

    this.draftState.clear();
    this.saved.emit();
  }

  cancel(): void {
    this.draftState.clear();
    this.cancelled.emit();
  }
}
