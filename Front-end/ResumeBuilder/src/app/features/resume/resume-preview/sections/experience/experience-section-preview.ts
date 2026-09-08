import { Component, input } from '@angular/core';
import { toEditorHtml } from '../../../../../shared/utils/rich-text';
import { ExperienceEntryInternalResponse } from '../../../../../core/models/resume-model';

@Component({
  selector: 'app-experience-section-preview',
  imports: [],
  templateUrl: './experience-section-preview.html',
})
export class ExperienceSectionPreview {
  entry = input.required<ExperienceEntryInternalResponse>();

  convertToDescriptionHtml(value: string | null): string {
    return value ? toEditorHtml(value) : '';
  }

  formatDateRange(startDate: string | null, endDate: string | null, isCurrent: boolean): string {
    const start = startDate ? this.formatMonthYear(startDate) : '';
    const end = isCurrent ? 'Present' : endDate ? this.formatMonthYear(endDate) : '';
    return start || end ? `${start} – ${end}` : '';
  }
  private formatMonthYear(isoDate: string): string {
    const date = new Date(isoDate);
    if (Number.isNaN(date.getTime())) {
      return '';
    }
    const month = `${date.getMonth() + 1}`.padStart(2, '0');
    return `${month}/${date.getFullYear()}`;
  }
}
