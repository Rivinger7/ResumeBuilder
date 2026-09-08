import { ResumeSectionInternalResponse, ResumeSectionType } from './resume-model';

/**
 * Maps ResumeSectionType to the field name holding its entry list in
 * ResumeSectionInternalResponse (educationEntries, experienceEntries...). Shared
 * between ResumePreview (merge draft) and ResumeDetail (optimistic reorder) so they
 * don't drift when a new section type is added.
 */
export const ENTRY_LIST_FIELDS: Partial<
  Record<ResumeSectionType, keyof ResumeSectionInternalResponse>
> = {
  [ResumeSectionType.Summary]: 'summaryEntries',
  [ResumeSectionType.Objective]: 'objectiveEntries',
  [ResumeSectionType.Education]: 'educationEntries',
  [ResumeSectionType.Experience]: 'experienceEntries',
  [ResumeSectionType.Projects]: 'projectEntries',
  [ResumeSectionType.Certificates]: 'certificateEntries',
  [ResumeSectionType.Languages]: 'languageEntries',
  [ResumeSectionType.Skills]: 'skillEntries',
};
