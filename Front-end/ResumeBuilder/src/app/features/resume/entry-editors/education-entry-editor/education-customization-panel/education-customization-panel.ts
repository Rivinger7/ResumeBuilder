import { Component, computed, input, output, signal } from '@angular/core';
import { EducationCustomization } from '../../interfaces/customization/customization-model';
import {
  ToggleButtonGroup,
  ToggleOption,
} from '../../../../../shared/components/toggle-button-group/toggle-button-group';

@Component({
  selector: 'app-education-customization-panel',
  imports: [ToggleButtonGroup],
  templateUrl: './education-customization-panel.html',
})
export class EducationCustomizationPanel {
  readonly customization = input.required<EducationCustomization>();
  readonly customizationChange = output<EducationCustomization>();

  readonly options: ToggleOption<boolean>[] = [
    {
      value: false,
      label: 'School, Degree',
    },
    {
      value: true,
      label: 'Degree, School',
    },
  ];

  readonly isByOrderStyle = computed(() => this.customization().isByOrder);

  emitChange(patch: Partial<EducationCustomization>): void {
    this.customizationChange.emit({ ...this.customization(), ...patch });
  }
}
