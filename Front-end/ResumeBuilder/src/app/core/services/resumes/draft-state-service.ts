import { Injectable, signal } from '@angular/core';
import { ResumeSectionType } from '../../models/resume-model';

/**
 * Holds EntryEditor's unsaved in-progress form value so ResumePreview can render it in
 * real time without waiting on an API round-trip. Only one entry is ever edited at a
 * time app-wide, so this state is global rather than per-section.
 *
 * EntryEditor writes the draft on every form change; ResumePreview reads and merges it
 * over the original resume() when rendering.
 */
export interface EntryDraft {
  sectionType: ResumeSectionType;
  /** id of the entry being edited; undefined when creating a new entry */
  entryId: string | undefined;
  /** Current form field values, keyed by BE field name (fullName, schoolName, summary...) */
  values: Record<string, unknown>;
}

@Injectable({ providedIn: 'root' })
export class DraftStateService {
  private readonly _draft = signal<EntryDraft | null>(null);
  readonly draft = this._draft.asReadonly();

  set(draft: EntryDraft): void {
    this._draft.set(draft);
  }

  clear(): void {
    this._draft.set(null);
  }
}
