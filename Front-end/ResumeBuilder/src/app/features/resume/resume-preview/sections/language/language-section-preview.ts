import { Component, input } from '@angular/core';
import { LanguageEntryInternalResponse } from '../../../../../core/models/resume-model';

@Component({
  selector: 'app-language-section-preview',
  imports: [],
  templateUrl: './language-section-preview.html',
})
export class LanguageSectionPreview {
  entry = input.required<LanguageEntryInternalResponse>();
}
