import { Component, input } from '@angular/core';
import { ObjectiveEntryInternalResponse } from '../../../../../core/models/resume-model';
import { toEditorHtml } from '../../../../../shared/utils/rich-text';

@Component({
  selector: 'app-objective-section-preview',
  imports: [],
  templateUrl: './objective-section-preview.html',
})
export class ObjectiveSectionPreview {
  entry = input.required<ObjectiveEntryInternalResponse>();

  convertToDescriptionHtml(value: string | null): string {
    return value ? toEditorHtml(value) : '';
  }
}
