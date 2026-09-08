import { Component, computed, effect, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Dialog } from '@angular/cdk/dialog';
import { CdkDropList, CdkDrag, CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { firstValueFrom } from 'rxjs';
import { ResumeService } from '../../../core/services/resumes/resume-service';
import { AuthenticationService } from '../../../core/services/authentication/authentication-service';
import {
  ResumeInternalResponse,
  ResumeSectionInternalResponse,
  ResumeSectionType,
} from '../../../core/models/resume-model';
import { ENTRY_LIST_FIELDS } from '../../../core/models/resume-entry-field-model';
import { ResumePreview } from '../resume-preview/resume-preview';
import { PersonalInfoCard } from '../personal-info-card/personal-info-card';
import { SectionCard } from '../section-card/section-card';
import { AddContentDialog } from '../add-content-dialog/add-content-dialog';
import { LucideIconModule } from '../../../shared/utils/lucide-icon-module';

const AUTO_SAVE_INTERVAL_MS = 5 * 60 * 1000;

@Component({
  selector: 'app-resume-detail',
  imports: [ResumePreview, PersonalInfoCard, SectionCard, CdkDropList, CdkDrag, LucideIconModule],
  templateUrl: './resume-detail.html',
})
export class ResumeDetail implements OnInit, OnDestroy {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly resumeService = inject(ResumeService);
  private readonly authService = inject(AuthenticationService);
  private readonly dialog = inject(Dialog);

  readonly resume = signal<ResumeInternalResponse | null>(null);
  readonly isDownloading = signal(false);
  readonly isSavingThumbnail = signal(false);

  readonly personalInfoSection = computed(() =>
    (this.resume()?.resumeSections ?? []).find(
      (s) => s.type === ResumeSectionType.PersonalInformation,
    ),
  );

  readonly otherSections = computed<ResumeSectionInternalResponse[]>(() =>
    (this.resume()?.resumeSections ?? [])
      .filter((s) => s.type !== ResumeSectionType.PersonalInformation)
      .slice()
      .sort((a, b) => a.displayOrder - b.displayOrder),
  );

  // Which card currently has its entry editor open, if any — while set, the left panel
  // shows only that card so editing gets the full column (see resume-detail.html).
  readonly editingSectionId = signal<'personal' | string | null>(null);

  readonly visibleSections = computed<ResumeSectionInternalResponse[]>(() => {
    const editing = this.editingSectionId();
    if (editing === null) return this.otherSections();
    if (editing === 'personal') return [];
    return this.otherSections().filter((s) => s.id === editing);
  });

  private resumeId = '';
  private hasLoadedOnce = false;
  private autoSaveTimer: ReturnType<typeof setInterval> | undefined;

  constructor() {
    // Redirect immediately on logout while on this page, instead of leaving it calling
    // the API with a cleared token.
    effect(() => {
      const isAuthenticated = this.authService.isAuthenticated();
      if (!isAuthenticated && this.hasLoadedOnce) {
        void this.router.navigate(['/home']);
      }
    });
  }

  async ngOnInit(): Promise<void> {
    this.resumeId = this.route.snapshot.paramMap.get('id') ?? '';
    if (this.resumeId) {
      await this.reload();
      this.hasLoadedOnce = true;
      this.autoSaveTimer = setInterval(() => void this.generateThumbnail(), AUTO_SAVE_INTERVAL_MS);
    }
  }

  ngOnDestroy(): void {
    if (this.autoSaveTimer) {
      clearInterval(this.autoSaveTimer);
    }
    // Generate a final thumbnail on unmount — fire-and-forget, don't block navigation.
    if (this.hasLoadedOnce) {
      void this.generateThumbnail();
    }
  }

  /** Explicit "Save" button — user commits the preview/thumbnail right away. */
  async saveNow(): Promise<void> {
    await this.generateThumbnail();
  }

  private async generateThumbnail(): Promise<void> {
    if (!this.resumeId) return;
    this.isSavingThumbnail.set(true);
    try {
      await this.resumeService.generateThumbnail(this.resumeId);
    } finally {
      this.isSavingThumbnail.set(false);
    }
  }

  private async reload(): Promise<void> {
    const resume = await this.resumeService.getById(this.resumeId);
    this.resume.set(resume);
  }

  async onSectionChanged(): Promise<void> {
    await this.reload();
  }

  async onSectionDeleted(sectionId: string): Promise<void> {
    await this.resumeService.deleteResumeSection(sectionId);
    await this.onSectionChanged();
  }

  onPersonalEditingChanged(editing: boolean): void {
    this.editingSectionId.set(editing ? 'personal' : null);
  }

  onSectionEditingChanged(sectionId: string, editing: boolean): void {
    this.editingSectionId.set(editing ? sectionId : null);
  }

  /**
   * Optimistic update when an entry is drag-reordered in SectionCard — updates displayOrder
   * directly in the resume() signal (SectionCard already calls entryService.reorderEntry in
   * the background) so ResumePreview reacts instantly instead of waiting on a reload.
   *
   * Shifts displayOrder for entries between the old and new position (matching
   * DisplayOrderService.cs on the BE) instead of just swapping one entry, to avoid two
   * entries sharing a displayOrder.
   */
  onEntryReordered(sectionId: string, event: { entryId: string; newDisplayOrder: number }): void {
    const current = this.resume();
    if (!current) return;

    const sections = current.resumeSections?.map((section) => {
      if (section.id !== sectionId) return section;

      const listField = ENTRY_LIST_FIELDS[section.type];
      if (!listField) return section;

      const list = section[listField] as { id: string; displayOrder: number }[] | null;
      if (!list) return section;

      const moving = list.find((entry) => entry.id === event.entryId);
      if (!moving) return section;

      const from = moving.displayOrder;
      const to = event.newDisplayOrder;
      if (from === to) return section;

      const updatedList = list.map((entry) => {
        if (entry.id === event.entryId) {
          return { ...entry, displayOrder: to };
        }
        if (from < to && entry.displayOrder > from && entry.displayOrder <= to) {
          return { ...entry, displayOrder: entry.displayOrder - 1 };
        }
        if (from > to && entry.displayOrder >= to && entry.displayOrder < from) {
          return { ...entry, displayOrder: entry.displayOrder + 1 };
        }
        return entry;
      });

      return { ...section, [listField]: updatedList };
    });

    this.resume.set({ ...current, resumeSections: sections ?? null });
  }

  async openAddContent(): Promise<void> {
    const existingTypes = this.otherSections().map((s) => s.type);
    const dialogRef = this.dialog.open<ResumeSectionType | undefined>(AddContentDialog, {
      data: { existingTypes },
    });

    const selectedType = await firstValueFrom(dialogRef.closed);
    if (!selectedType) {
      return;
    }

    const maxDisplayOrder = this.otherSections().reduce(
      (max, s) => Math.max(max, s.displayOrder),
      1,
    );

    await this.resumeService.createResumeSection({
      resumeId: this.resumeId,
      type: selectedType,
      displayOrder: maxDisplayOrder + 1,
    });

    await this.onSectionChanged();
  }

  async onSectionDrop(event: CdkDragDrop<ResumeSectionInternalResponse[]>): Promise<void> {
    if (event.previousIndex === event.currentIndex) {
      return;
    }

    const reordered = this.otherSections().slice();
    moveItemInArray(reordered, event.previousIndex, event.currentIndex);
    const moved = reordered[event.currentIndex];

    // PersonalInformation always occupies displayOrder 1 — other sections start at 2.
    await this.resumeService.reorderSection(this.resumeId, moved.id, event.currentIndex + 2);
    await this.onSectionChanged();
  }

  async downloadPdf(): Promise<void> {
    const resume = this.resume();
    if (!resume) {
      return;
    }

    this.isDownloading.set(true);
    try {
      await this.resumeService.downloadPdf(this.resumeId, `${resume.title}.pdf`);
    } finally {
      this.isDownloading.set(false);
    }
  }
}
