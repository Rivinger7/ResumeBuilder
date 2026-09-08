import { Component, input } from '@angular/core';
import { toEditorHtml } from '../../../../../shared/utils/rich-text';
import { SummaryEntryInternalResponse } from '../../../../../core/models/resume-model';

@Component({
  selector: 'app-summary-section-preview',
  imports: [],
  templateUrl: './summary-section-preview.html',
})
export class SummarySectionPreview {
  entry = input.required<SummaryEntryInternalResponse>();

  convertToDescriptionHtml(value: string | null): string {
    return value ? toEditorHtml(value) : '';
  }
}
