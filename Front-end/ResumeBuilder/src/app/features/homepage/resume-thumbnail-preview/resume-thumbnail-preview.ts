import {
  Component,
  DestroyRef,
  ElementRef,
  afterNextRender,
  inject,
  input,
  signal,
} from '@angular/core';
import { ResumePreview } from '../../resume/resume-preview/resume-preview';
import { ResumeInternalResponse } from '../../../core/models/resume-model';

// Reference width the preview renders at internally before being scaled down to fit
// the card, so text/layout proportions stay accurate at any card size.
const PREVIEW_WIDTH_PX = 850;

@Component({
  selector: 'app-resume-thumbnail-preview',
  imports: [ResumePreview],
  templateUrl: './resume-thumbnail-preview.html',
})
export class ResumeThumbnailPreview {
  private readonly elementRef = inject(ElementRef<HTMLElement>);
  private readonly destroyRef = inject(DestroyRef);

  readonly resume = input<ResumeInternalResponse | undefined>(undefined);
  readonly previewWidth = PREVIEW_WIDTH_PX;

  // Starts at 0 (renders nothing) until measured — avoids a flash of the unscaled,
  // full-size preview.
  readonly scale = signal(0);

  constructor() {
    afterNextRender(() => {
      const host = this.elementRef.nativeElement;

      // Measure synchronously first — ResizeObserver's own initial callback is tied to
      // the render/composite loop and won't fire while the tab is backgrounded.
      this.scale.set(host.getBoundingClientRect().width / PREVIEW_WIDTH_PX);

      const observer = new ResizeObserver(([entry]) => {
        this.scale.set(entry.contentRect.width / PREVIEW_WIDTH_PX);
      });
      observer.observe(host);
      this.destroyRef.onDestroy(() => observer.disconnect());
    });
  }
}
