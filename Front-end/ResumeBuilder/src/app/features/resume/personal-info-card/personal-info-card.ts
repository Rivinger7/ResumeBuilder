import { Component, computed, effect, inject, input, output, signal } from '@angular/core';
import { LucideIconModule } from '../../../shared/utils/lucide-icon-module';
import { PersonalInformationEntryEditor } from '../entry-editors/personal-information-entry-editor/personal-information-entry-editor';
import { ApiClientService } from '../../../core/services/api-client-service';
import { EntryService } from '../../../core/services/resumes/entry-service';
import {
  IconStyleType,
  LayoutType,
  MarkerStyleType,
  PersonalInformationEntryInternalResponse,
  TextAlignmentType,
} from '../../../core/models/resume-model';
import { PersonalInformationCustomizationPanel } from '../entry-editors/personal-information-entry-editor/personal-information-customization-panel/personal-information-customization-panel';
import { PersonalInformationCustomization } from '../entry-editors/interfaces/customization/customization-model';

@Component({
  selector: 'app-personal-info-card',
  imports: [
    PersonalInformationEntryEditor,
    LucideIconModule,
    PersonalInformationCustomizationPanel,
  ],
  templateUrl: './personal-info-card.html',
})
export class PersonalInfoCard {
  private readonly apiClient = inject(ApiClientService);
  private readonly entryService = inject(EntryService);

  readonly PencilIcon = 'pencil';
  readonly MailIcon = 'mail';
  readonly PhoneIcon = 'phone';
  readonly MapPinIcon = 'map-pin';
  readonly CameraIcon = 'camera';

  readonly resumeSectionId = input.required<string>();
  readonly personalInfo = input<PersonalInformationEntryInternalResponse | null>(null);

  readonly changed = output<void>();
  readonly editingChanged = output<boolean>();

  readonly isEditing = signal(false);
  readonly showCustomization = signal(false);

  readonly photoSrc = computed<string | null>(() => {
    const photoUrl = this.personalInfo()?.photoUrl;
    return photoUrl ? `${this.apiClient.origin}${photoUrl}` : null;
  });

  // Local, optimistically-updated copy — reflects clicks immediately instead of
  // waiting on a full resume reload, which would otherwise make clicks look inert.
  readonly customization = signal<PersonalInformationCustomization>({
    textAlignment: TextAlignmentType.Start,
    layout: LayoutType.Grid,
    markerStyle: MarkerStyleType.Icon,
    iconStyle: IconStyleType.Default,
  });

  constructor() {
    effect(() => {
      const info = this.personalInfo();
      this.customization.set({
        textAlignment: info?.personalInformationTextAlignment ?? TextAlignmentType.Start,
        layout: info?.personalInformationLayout ?? LayoutType.Grid,
        markerStyle: info?.personalInformationMarkerStyle ?? MarkerStyleType.Icon,
        iconStyle: info?.personalInformationIconStyle ?? IconStyleType.Default,
      });
    });
  }

  edit(): void {
    this.isEditing.set(true);
    this.editingChanged.emit(true);
  }

  onSaved(): void {
    this.isEditing.set(false);
    this.editingChanged.emit(false);
    this.changed.emit();
  }

  onCancelled(): void {
    this.isEditing.set(false);
    this.editingChanged.emit(false);
  }

  toggleCustomization(): void {
    this.showCustomization.set(!this.showCustomization());
  }

  async onCustomizationChange(value: PersonalInformationCustomization): Promise<void> {
    this.customization.set(value);

    const id = this.personalInfo()?.id;
    if (!id) return;

    await this.entryService.updatePersonalInformationStyle(id, {
      personalInformationTextAlignment: value.textAlignment,
      personalInformationLayout: value.layout,
      personalInformationMarkerStyle: value.markerStyle,
      personalInformationIconStyle: value.iconStyle,
    });
    this.changed.emit();
  }
}
