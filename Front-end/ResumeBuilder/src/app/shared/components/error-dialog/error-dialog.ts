import { Component, inject } from '@angular/core';
import { BrnDialogRef, injectBrnDialogContext } from '@spartan-ng/brain/dialog';
import { HlmDialogImports } from '@spartan-ng/helm/dialog';
import { HlmButtonImports } from '@spartan-ng/helm/button';

export interface ErrorDialogContext {
  title: string;
  message: string;
}

@Component({
  selector: 'app-error-dialog',
  standalone: true,
  imports: [HlmDialogImports, HlmButtonImports],
  template: `
    <hlm-dialog-header>
      <h3 hlmDialogTitle>{{ context.title }}</h3>
      <p hlmDialogDescription>{{ context.message }}</p>
    </hlm-dialog-header>

    <hlm-dialog-footer>
      <button hlmBtn (click)="close()">Đã hiểu</button>
    </hlm-dialog-footer>
  `,
})
export class ErrorDialogComponent {
  protected readonly context = injectBrnDialogContext<ErrorDialogContext>();
  private readonly dialogRef = inject(BrnDialogRef);

  close(): void {
    this.dialogRef.close();
  }
}
