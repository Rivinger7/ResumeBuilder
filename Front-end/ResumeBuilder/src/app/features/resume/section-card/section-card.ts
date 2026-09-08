import { Component, computed, inject, input, output, signal } from '@angular/core';
import {
  CdkDropList,
  CdkDrag,
  CdkDragDrop,
  CdkDragHandle,
  moveItemInArray,
} from '@angular/cdk/drag-drop';
import { LucideIconModule } from '../../../shared/utils/lucide-icon-module';
import { EducationEntryEditor } from '../entry-editors/education-entry-editor/education-entry-editor';
import { ExperienceEntryEditor } from '../entry-editors/experience-entry-editor/experience-entry-editor';
import { ProjectEntryEditor } from '../entry-editors/project-entry-editor/project-entry-editor';
import { CertificateEntryEditor } from '../entry-editors/certificate-entry-editor/certificate-entry-editor';
import { LanguageEntryEditor } from '../entry-editors/language-entry-editor/language-entry-editor';
import { SkillEntryEditor } from '../entry-editors/skill-entry-editor/skill-entry-editor';
import { ObjectiveEntryEditor } from '../entry-editors/objective-entry-editor/objective-entry-editor';
import { SummaryEntryEditor } from '../entry-editors/summary-entry-editor/summary-entry-editor';
import { EntryService } from '../../../core/services/resumes/entry-service';
import { DraftStateService } from '../../../core/services/resumes/draft-state-service';
import { SECTION_ICONS } from '../resume-section-icons';
import {
  ResumeSectionInternalResponse,
  ResumeSectionType,
} from '../../../core/models/resume-model';
import { toEditorHtml } from '../../../shared/utils/rich-text';

type EntryRow = { id: string; summary: string; raw: unknown };

@Component({
  selector: 'app-section-card',
  imports: [
    LucideIconModule,
    CdkDropList,
    CdkDrag,
    CdkDragHandle,
    EducationEntryEditor,
    ExperienceEntryEditor,
    ProjectEntryEditor,
    CertificateEntryEditor,
    LanguageEntryEditor,
    SkillEntryEditor,
    ObjectiveEntryEditor,
    SummaryEntryEditor,
  ],
  templateUrl: './section-card.html',
  styleUrl: './section-card.css',
})
export class SectionCard {
  private readonly entryService = inject(EntryService);
  private readonly draftState = inject(DraftStateService);

  readonly ChevronDownIcon = 'chevron-down';
  readonly ChevronUpIcon = 'chevron-up';
  readonly GripVerticalIcon = 'grip-vertical';
  readonly EyeIcon = 'eye';
  readonly TrashIcon = 'trash-2';
  readonly ResumeSectionType = ResumeSectionType;

  readonly section = input.required<ResumeSectionInternalResponse>();

  readonly changed = output<void>();
  readonly reordered = output<{ entryId: string; newDisplayOrder: number }>();
  readonly deleted = output<void>();
  readonly editingChanged = output<boolean>();

  readonly isOpen = signal(true);
  readonly editingEntry = signal<string | 'new' | null>(null);

  readonly icon = computed(() => SECTION_ICONS[this.section().type]);

  readonly entries = computed<EntryRow[]>(() => {
    const section = this.section();
    const list = entriesFor(section);
    return list
      .slice()
      .sort((a, b) => (a['displayOrder'] as number) - (b['displayOrder'] as number))
      .map((raw) => ({
        id: raw['id'] as string,
        summary: summaryFor(section.type, raw),
        raw,
      }));
  });

  /** Renders a description field's HTML; legacy plain-text values are migrated on the fly. */
  descriptionHtml(value: string | null): string {
    return value ? toEditorHtml(value) : '';
  }

  toggle(): void {
    this.isOpen.set(!this.isOpen());
  }

  removeSection(event: Event): void {
    event.stopPropagation();
    this.deleted.emit();
  }

  openEntry(entry: EntryRow): void {
    this.draftState.clear();
    this.editingEntry.set(entry.id);
    this.editingChanged.emit(true);
  }

  addEntry(): void {
    this.draftState.clear();
    this.editingEntry.set('new');
    this.editingChanged.emit(true);
  }

  onSaved(): void {
    this.editingEntry.set(null);
    this.editingChanged.emit(false);
    this.changed.emit();
  }

  onCancelled(): void {
    this.editingEntry.set(null);
    this.editingChanged.emit(false);
    this.changed.emit();
  }

  onDeleted(): void {
    this.editingEntry.set(null);
    this.editingChanged.emit(false);
    this.changed.emit();
  }

  editingEntryRaw(): unknown {
    const editing = this.editingEntry();
    if (editing === 'new' || editing === null) return null;
    return this.entries().find((entry) => entry.id === editing)?.raw ?? null;
  }

  async onEntryDrop(event: CdkDragDrop<EntryRow[]>): Promise<void> {
    if (event.previousIndex === event.currentIndex) {
      return;
    }

    const reordered = this.entries().slice();
    moveItemInArray(reordered, event.previousIndex, event.currentIndex);
    const moved = reordered[event.currentIndex];
    const newDisplayOrder = event.currentIndex + 1;

    // Tell ResumeDetail to update resume() right away (optimistic) so ResumePreview
    // reacts instantly instead of waiting on the API + reload.
    this.reordered.emit({ entryId: moved.id, newDisplayOrder });

    try {
      await this.entryService.reorderEntry(
        this.section().id,
        moved.id,
        this.section().type,
        newDisplayOrder,
      );
    } catch (error) {
      // API failed — the optimistic update is now out of sync, force a reload.
      this.changed.emit();
      throw error;
    }
  }
}

function entriesFor(section: ResumeSectionInternalResponse): Record<string, unknown>[] {
  switch (section.type) {
    case ResumeSectionType.Education:
      return (section.educationEntries ?? []) as unknown as Record<string, unknown>[];
    case ResumeSectionType.Experience:
      return (section.experienceEntries ?? []) as unknown as Record<string, unknown>[];
    case ResumeSectionType.Projects:
      return (section.projectEntries ?? []) as unknown as Record<string, unknown>[];
    case ResumeSectionType.Certificates:
      return (section.certificateEntries ?? []) as unknown as Record<string, unknown>[];
    case ResumeSectionType.Languages:
      return (section.languageEntries ?? []) as unknown as Record<string, unknown>[];
    case ResumeSectionType.Skills:
      return (section.skillEntries ?? []) as unknown as Record<string, unknown>[];
    case ResumeSectionType.Summary:
      return (section.summaryEntries ?? []) as unknown as Record<string, unknown>[];
    case ResumeSectionType.Objective:
      return (section.objectiveEntries ?? []) as unknown as Record<string, unknown>[];
    default:
      return [];
  }
}

function summaryFor(type: ResumeSectionType, entry: Record<string, unknown>): string {
  switch (type) {
    case ResumeSectionType.Education:
      return (entry['schoolName'] as string) || 'New Entry';
    case ResumeSectionType.Experience:
      return [entry['position'], entry['companyName']].filter(Boolean).join(' — ') || 'New Entry';
    case ResumeSectionType.Projects:
      return (entry['title'] as string) || 'New Entry';
    case ResumeSectionType.Certificates:
      return (entry['title'] as string) || 'New Entry';
    case ResumeSectionType.Languages:
      return (entry['languageName'] as string) || 'New Entry';
    case ResumeSectionType.Skills:
      return (entry['skillName'] as string) || 'New Entry';
    case ResumeSectionType.Summary:
      return (entry['summary'] as string) || 'New Entry';
    case ResumeSectionType.Objective:
      return (entry['title'] as string) || (entry['description'] as string) || 'New Entry';
    default:
      return 'New Entry';
  }
}
