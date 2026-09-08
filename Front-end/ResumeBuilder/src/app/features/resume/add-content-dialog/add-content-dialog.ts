import { Component, inject } from '@angular/core';
import { DialogRef, DIALOG_DATA } from '@angular/cdk/dialog';
import { LucideIconModule } from '../../../shared/utils/lucide-icon-module';
import { SECTION_ICONS, SECTION_DESCRIPTIONS } from '../resume-section-icons';
import { ResumeSectionType } from '../../../core/models/resume-model';

const ALL_ADDABLE_TYPES = Object.values(ResumeSectionType).filter(
  (type) => type !== ResumeSectionType.PersonalInformation,
);

@Component({
  selector: 'app-add-content-dialog',
  imports: [LucideIconModule],
  templateUrl: './add-content-dialog.html',
})
export class AddContentDialog {
  private readonly dialogRef = inject(DialogRef<ResumeSectionType | undefined>);
  private readonly data = inject<{ existingTypes: ResumeSectionType[] }>(DIALOG_DATA);

  readonly XIcon = 'x';
  readonly SECTION_ICONS = SECTION_ICONS;
  readonly SECTION_DESCRIPTIONS = SECTION_DESCRIPTIONS;

  readonly options = ALL_ADDABLE_TYPES.filter((type) => !this.data.existingTypes.includes(type));

  select(type: ResumeSectionType): void {
    this.dialogRef.close(type);
  }

  close(): void {
    this.dialogRef.close(undefined);
  }
}
