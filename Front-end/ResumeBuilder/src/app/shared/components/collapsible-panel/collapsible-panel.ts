import { Component, input, signal } from '@angular/core';
import { LucideIconModule } from '../../utils/lucide-icon-module';

@Component({
  selector: 'app-collapsible-panel',
  imports: [LucideIconModule],
  templateUrl: './collapsible-panel.html',
})
export class CollapsiblePanel {
  readonly title = input.required<string>();
  readonly initialOpen = input(false);
  readonly isOpen = signal(false);

  constructor() {
    const init = this.initialOpen;
    if (init()) this.isOpen.set(true);
  }

  toggle(): void {
    this.isOpen.set(!this.isOpen());
  }
}
