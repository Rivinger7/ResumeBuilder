import { Component, computed, inject, input } from '@angular/core';
import { NgStyle } from '@angular/common';
import { DraftStateService } from '../../../core/services/resumes/draft-state-service';
import { ENTRY_LIST_FIELDS } from '../../../core/models/resume-entry-field-model';
import {
  LayoutType,
  ResumeInternalResponse,
  ResumeSectionInternalResponse,
  ResumeSectionType,
} from '../../../core/models/resume-model';
import { LucideIconModule } from '../../../shared/utils/lucide-icon-module';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faGithub, faLinkedin } from '@fortawesome/free-brands-svg-icons';
import { PersonalInformationSecitonPreview } from './sections/personal-information/personal-information-seciton-preview';
import { SummarySectionPreview } from './sections/summary/summary-section-preview';
import { ObjectiveSectionPreview } from './sections/objective/objective-section-preview';
import { EducationSectionPreview } from './sections/education/education-section-preview';
import { ExperienceSectionPreview } from './sections/experience/experience-section-preview';
import { ProjectSectionPreview } from './sections/project/project-section-preview';
import { CertificateSectionPreview } from './sections/certificate/certificate-section-preview';
import { LanguageSectionPreview } from './sections/language/language-section-preview';
import { SkillSectionPreview } from './sections/skill/skill-section-preview';
import { ACCENT_COLOR } from '../../../shared/models/color';

// Layout mirrors ResumeDocument.cs (BE) — navy header, underlined section titles,
// spacing/typography from resume.settings — so the live preview matches the PDF export.

const ptToPxConversionConstant = 96 / 72;

@Component({
  selector: 'app-resume-preview',
  imports: [
    NgStyle,
    LucideIconModule,
    FontAwesomeModule,
    PersonalInformationSecitonPreview,
    SummarySectionPreview,
    ObjectiveSectionPreview,
    EducationSectionPreview,
    ExperienceSectionPreview,
    ProjectSectionPreview,
    CertificateSectionPreview,
    LanguageSectionPreview,
    SkillSectionPreview,
  ],
  templateUrl: './resume-preview.html',
})
export class ResumePreview {
  private readonly draftState = inject(DraftStateService);

  readonly resume = input<ResumeInternalResponse | undefined>(undefined);

  readonly ResumeSectionType = ResumeSectionType;
  faGithub = faGithub;
  faLinkedin = faLinkedin;

  // Resume with the in-progress draft (if any) merged over the original data — every
  // computed below reads from here instead of resume() directly, for real-time preview.
  private readonly effectiveResume = computed<ResumeInternalResponse | undefined>(() => {
    const resume = this.resume();
    const draft = this.draftState.draft();
    if (!resume || !draft) {
      return resume;
    }

    const sections = (resume.resumeSections ?? []).map((section) => {
      if (section.type !== draft.sectionType) {
        return section;
      }

      if (section.type === ResumeSectionType.PersonalInformation) {
        const current = section.personalInformationEntries?.[0];
        // No existing entry — PersonalInformation always exists already, so skip.
        if (!current) return section;
        return {
          ...section,
          personalInformationEntries: [{ ...current, ...draft.values } as typeof current],
        };
      }

      const listField = ENTRY_LIST_FIELDS[section.type];
      if (!listField) return section;

      const list = (section[listField] as { id: string }[] | null) ?? [];

      if (draft.entryId) {
        // Editing an existing entry — merge the draft into it by id.
        const updatedList = list.map((entry) =>
          entry.id === draft.entryId ? { ...entry, ...draft.values } : entry,
        );
        return { ...section, [listField]: updatedList };
      }

      // Creating a new entry — append a temp entry (empty id) so the preview shows it right away.
      return { ...section, [listField]: [...list, { id: '', ...draft.values }] };
    });

    return { ...resume, resumeSections: sections };
  });

  readonly personalInfo = computed(() => {
    const section = (this.effectiveResume()?.resumeSections ?? []).find(
      (s) => s.type === ResumeSectionType.PersonalInformation,
    );
    return section?.personalInformationEntries?.[0];
  });
  readonly personalInfoSettings = computed(() => {
    return this.resume()?.settings?.backgroundColorHeader;
  });

  readonly sections = computed<ResumeSectionInternalResponse[]>(() => {
    return (this.effectiveResume()?.resumeSections ?? [])
      .filter((s) => s.type !== ResumeSectionType.PersonalInformation)
      .slice()
      .sort((a, b) => a.displayOrder - b.displayOrder)
      .map((section) => this.sortEntriesInSection(section));
  });
  /**
   * Sorts a section's entry list by displayOrder before render — required because @for in
   * resume-preview.html iterates the array as-is, and drag-reorder only mutates
   * displayOrder, not array order. SectionCard.entries sorts the same way for its own list.
   */
  private sortEntriesInSection(
    section: ResumeSectionInternalResponse,
  ): ResumeSectionInternalResponse {
    const listField = ENTRY_LIST_FIELDS[section.type];
    if (!listField) return section;

    const list = section[listField] as { displayOrder: number }[] | null;
    if (!list) return section;

    const sorted = list.slice().sort((a, b) => a.displayOrder - b.displayOrder);
    return { ...section, [listField]: sorted };
  }

  readonly containerStyle = computed(() => {
    const settings = this.resume()?.settings;
    return {
      'font-family': settings?.font ?? 'inherit',
      'font-size': `${(settings?.fontSize ?? 11) * ptToPxConversionConstant}px`,
      'line-height': `${settings?.lineHeight ?? 1.4}`,
    };
  });

  readonly bodyStyle = computed(() => {
    const settings = this.resume()?.settings;
    return {
      'padding-left': `${(settings?.leftRightMargin ?? 24) * ptToPxConversionConstant}px`,
      'padding-right': `${(settings?.leftRightMargin ?? 24) * ptToPxConversionConstant}px`,
      'row-gap': `${settings?.spaceBetweenElements ?? 12}px`,
    };
  });

  readonly sectionTitleStyle = computed(() => {
    const settings = this.resume()?.settings;
    return {
      color: `${settings?.sectionTitleColor ?? ACCENT_COLOR}`,
      'font-size': `${(settings?.sectionTitleFontSize ?? 14) * ptToPxConversionConstant}px`,
    };
  });

  certificateLayoutStyle(section: ResumeSectionInternalResponse): Record<string, string> {
    const entries = section.certificateEntries ?? [];
    const draft = this.draftState.draft();
    const draftEntry =
      draft?.sectionType === ResumeSectionType.Certificates
        ? (draft.values as Partial<{
            certificateLayout: LayoutType;
            certificateGridColumn: number | null;
            certificateRowSpacing: string | null;
            isStartRowsWithBullet: boolean;
          }>)
        : undefined;
    const activeEntry = draftEntry ?? entries[0];
    const layout = activeEntry?.certificateLayout ?? LayoutType.Grid;
    const columns = activeEntry?.certificateGridColumn ?? 2;
    const rowSpacing = activeEntry?.certificateRowSpacing;
    let rowSpacingPx = '4px';
    const isStartRowsWithBullet = activeEntry?.isStartRowsWithBullet ?? false;

    if (layout === LayoutType.Grid) {
      return {
        display: 'grid',
        'grid-template-columns': `repeat(${columns}, 1fr)`,
        'column-gap': '12px',
      };
    } else if (layout === LayoutType.Rows) {
      if (rowSpacing === 'Spacious') {
        rowSpacingPx = '12px';
      } else if (rowSpacing === 'Tight') {
        rowSpacingPx = '4px';
      }
      return {
        display: 'flex',
        'flex-direction': 'column',
        'row-gap': rowSpacingPx,
        'list-style-type': isStartRowsWithBullet ? 'disc' : 'none',
        'padding-left': isStartRowsWithBullet ? '1.25rem' : '0',
      };
    }
    return {};
  }
}
