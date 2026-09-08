import { Component, computed, input } from '@angular/core';
import { toEditorHtml } from '../../../../../shared/utils/rich-text';
import {
  CertificateEntryInternalResponse,
  LayoutType,
} from '../../../../../core/models/resume-model';
import { LucideIconModule } from '../../../../../shared/utils/lucide-icon-module';

@Component({
  selector: 'app-certificate-section-preview',
  imports: [LucideIconModule],
  templateUrl: './certificate-section-preview.html',
})
export class CertificateSectionPreview {
  entry = input.required<CertificateEntryInternalResponse>();

  convertTodescriptionHtml(value: string | null): string {
    return value ? toEditorHtml(value) : '';
  }
}
