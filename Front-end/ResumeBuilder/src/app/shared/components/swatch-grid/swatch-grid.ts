import { Component, input, output } from '@angular/core';
import { LucideIconModule } from '../../utils/lucide-icon-module';

export interface SwatchOption<T = string> {
  value: T;
  icon: string;
  label?: string;
}

@Component({
  selector: 'app-swatch-grid',
  imports: [LucideIconModule],
  templateUrl: './swatch-grid.html',
})
export class SwatchGrid<T = string> {
  readonly options = input.required<SwatchOption<T>[]>();
  readonly value = input.required<T>();
  readonly changed = output<T>();

  select(option: SwatchOption<T>): void {
    if (option.value !== this.value()) {
      this.changed.emit(option.value);
    }
  }
}
