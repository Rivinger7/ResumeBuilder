import { Component, input } from '@angular/core';
import { SkillEntryInternalResponse } from '../../../../../core/models/resume-model';

@Component({
  selector: 'app-skill-section-preview',
  imports: [],
  templateUrl: './skill-section-preview.html',
})
export class SkillSectionPreview {
  entry = input.required<SkillEntryInternalResponse>();
}
