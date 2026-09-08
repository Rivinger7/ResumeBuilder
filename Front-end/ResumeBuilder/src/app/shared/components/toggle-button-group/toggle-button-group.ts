import { Component, input, output } from '@angular/core';
import { LucideIconModule } from '../../utils/lucide-icon-module';

export interface ToggleOption<T = string> {
  value: T;
  label?: string;
  icon?: string;
  preview?: string;
}

@Component({
  selector: 'app-toggle-button-group',
  imports: [LucideIconModule],
  templateUrl: './toggle-button-group.html',
})
export class ToggleButtonGroup<T = string> {
  readonly options = input.required<ToggleOption<T>[]>();
  readonly value = input.required<T>();
  readonly columns = input<number>(0);
  readonly size = input<'sm' | 'md'>('md');
  readonly changed = output<T>();

  select(option: ToggleOption<T>): void {
    if (option.value !== this.value()) {
      this.changed.emit(option.value);
    }
  }
}
