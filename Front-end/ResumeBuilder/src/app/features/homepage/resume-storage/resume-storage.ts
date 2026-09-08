import { Component, DestroyRef, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { LucideIconModule } from '../../../shared/utils/lucide-icon-module';
import { Dialog } from '@angular/cdk/dialog';
import { firstValueFrom, filter } from 'rxjs';
import { NavigationEnd, Router } from '@angular/router';
import { ResumeService } from '../../../core/services/resumes/resume-service';
import { CreateResumeDialog } from '../create-resume-dialog/create-resume-dialog';
import { ResumeThumbnailPreview } from '../resume-thumbnail-preview/resume-thumbnail-preview';
import { CreateResumeResponse } from '../../../core/models/resume-model';

// generate-thumbnail on the detail page is fire-and-forget on unmount, so it may still be
// in flight when we navigate back here; wait briefly before refetching.
const REFETCH_DELAY_MS = 1000;

@Component({
  selector: 'app-resume-storage',
  imports: [LucideIconModule, ResumeThumbnailPreview],
  templateUrl: './resume-storage.html',
})
export class ResumeStorage {
  readonly EllipsisVerticalIcon = 'ellipsis-vertical';
  readonly ArrowRightIcon = 'arrow-right';
  readonly CopyIcon = 'copy';
  readonly FilesIcon = 'files';

  private readonly resumeService = inject(ResumeService);
  private readonly router = inject(Router);
  private readonly dialog = inject(Dialog);
  private readonly destroyRef = inject(DestroyRef);

  resumes = this.resumeService.resumes;

  private pendingRefetch: ReturnType<typeof setTimeout> | undefined;

  constructor() {
    void this.resumeService.loadAllResumes();

    // Listen on NavigationEnd instead of just ngOnInit — Angular Router may reuse this
    // component instance when navigating back to the list route (no destroy/recreate),
    // so ngOnInit alone wouldn't rerun.
    this.router.events
      .pipe(
        filter((e): e is NavigationEnd => e instanceof NavigationEnd),
        filter((e) => this.isResumeListRoute(e.urlAfterRedirects)),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe(() => {
        void this.resumeService.loadAllResumes();

        clearTimeout(this.pendingRefetch);
        this.pendingRefetch = setTimeout(
          () => void this.resumeService.loadAllResumes(),
          REFETCH_DELAY_MS,
        );
      });

    this.destroyRef.onDestroy(() => clearTimeout(this.pendingRefetch));
  }

  private isResumeListRoute(url: string): boolean {
    const path = url.split('?')[0];
    return path === '/' || path === '' || path === '/home';
  }

  async createResume(): Promise<void> {
    const dialogRef = this.dialog.open<CreateResumeResponse | undefined>(CreateResumeDialog);
    const created = await firstValueFrom(dialogRef.closed);
    if (!created) {
      return;
    }

    await this.router.navigate(['/resumes', created.id, 'edit']);
  }

  async deleteResume(id: string, event: Event): Promise<void> {
    event.stopPropagation();
    await this.resumeService.delete(id);
    await this.resumeService.loadAllResumes();
  }

  viewResume(id: string): void {
    void this.router.navigate(['/resumes', id, 'edit']);
  }
}
