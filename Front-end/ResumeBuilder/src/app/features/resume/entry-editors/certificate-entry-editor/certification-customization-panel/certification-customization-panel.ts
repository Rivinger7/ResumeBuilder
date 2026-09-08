import { Component, input, output } from '@angular/core';
import {
  ToggleButtonGroup,
  ToggleOption,
} from '../../../../../shared/components/toggle-button-group/toggle-button-group';
import { CertificationCustomization } from '../../interfaces/customization/customization-model';
import { LayoutType } from '../../../../../core/models/resume-model';

@Component({
  selector: 'app-certification-customization-panel',
  imports: [ToggleButtonGroup],
  templateUrl: './certification-customization-panel.html',
})
export class CertificationCustomizationPanel {
  readonly customization = input.required<CertificationCustomization>();
  readonly customizationChange = output<CertificationCustomization>();

  readonly layoutOptions: ToggleOption<LayoutType>[] = [
    { value: LayoutType.Grid, icon: 'rectangle-horizontal' },
    { value: LayoutType.Rows, icon: 'list' },
  ];

  readonly gridColumnOptions: ToggleOption<number>[] = [
    { value: 1, label: '1 Column' },
    { value: 2, label: '2 Columns' },
    { value: 3, label: '3 Columns' },
    { value: 4, label: '4 Columns' },
  ];

  readonly rowSpacingOptions: ToggleOption<string>[] = [
    { value: 'Tight', label: 'Tight' },
    { value: 'Spacious', label: 'Spacious' },
  ];

  readonly subinfoStyleOptions: ToggleOption<string>[] = [
    { value: 'Default', label: 'Default' },
    { value: 'Bracket', label: 'Bracket' },
  ];

  emitChange(patch: Partial<CertificationCustomization>): void {
    this.customizationChange.emit({ ...this.customization(), ...patch });
  }

  onLayoutChange(layout: LayoutType): void {
    this.emitChange({
      certificateLayout: layout,
      certificateRowSpacing:
        layout === LayoutType.Rows
          ? (this.customization().certificateRowSpacing ?? 'Tight')
          : this.customization().certificateRowSpacing,
    });
  }
}
