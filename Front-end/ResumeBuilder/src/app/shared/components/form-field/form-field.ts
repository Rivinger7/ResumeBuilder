import { Component, input } from '@angular/core';

@Component({
  selector: 'app-form-field',
  template: `
    <label class="flex flex-col gap-y-1">
      <span class="text-sm font-bold">{{ label() }}</span>
      <ng-content />
    </label>
  `,
})
export class FormField {
  readonly label = input.required<string>();
}
