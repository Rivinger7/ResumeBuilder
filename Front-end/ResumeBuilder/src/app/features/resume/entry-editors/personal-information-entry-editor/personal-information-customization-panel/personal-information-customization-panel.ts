import { Component, computed, input, output } from '@angular/core';
import {
  ToggleButtonGroup,
  ToggleOption,
} from '../../../../../shared/components/toggle-button-group/toggle-button-group';
import { SwatchGrid, SwatchOption } from '../../../../../shared/components/swatch-grid/swatch-grid';
import {
  IconStyleType,
  LayoutType,
  MarkerStyleType,
  TextAlignmentType,
} from '../../../../../core/models/resume-model';
import { PersonalInformationCustomization } from '../../interfaces/customization/customization-model';

// Customization options specific to the Header (PersonalInformation) section — its own
// component rather than a generic "section customization" one, since every section type
// ends up needing a different set of controls.

@Component({
  selector: 'app-personal-information-customization-panel',
  imports: [ToggleButtonGroup, SwatchGrid],
  templateUrl: './personal-information-customization-panel.html',
})
export class PersonalInformationCustomizationPanel {
  readonly customization = input.required<PersonalInformationCustomization>();
  readonly customizationChange = output<PersonalInformationCustomization>();

  readonly alignmentOptions: ToggleOption<TextAlignmentType>[] = [
    { value: TextAlignmentType.Start, label: 'Left', icon: 'text-align-start' },
    { value: TextAlignmentType.Center, label: 'Center', icon: 'text-align-center' },
  ];

  readonly layoutOptions: ToggleOption<LayoutType>[] = [
    { value: LayoutType.Rows, icon: 'rectangle-horizontal' },
    { value: LayoutType.Grid, icon: 'list' },
  ];

  readonly markerOptions: ToggleOption<MarkerStyleType>[] = [
    { value: MarkerStyleType.Icon, label: 'Icon', icon: 'smile' },
    { value: MarkerStyleType.Bullet, label: 'Bullet' },
    { value: MarkerStyleType.Bar, label: 'Bar' },
  ];

  readonly iconStyleOptions: SwatchOption<IconStyleType>[] = [
    { value: IconStyleType.Default, icon: 'link', label: 'Default' },
    { value: IconStyleType.CircleFilled, icon: 'link', label: 'Circle Filled' },
    { value: IconStyleType.RoundedFilled, icon: 'link', label: 'Rounded Filled' },
    { value: IconStyleType.SquareFilled, icon: 'link', label: 'Square Filled' },
    { value: IconStyleType.CircleOutline, icon: 'link', label: 'Circle Outline' },
    { value: IconStyleType.RoundedOutline, icon: 'link', label: 'Rounded Outline' },
    { value: IconStyleType.SquareOutline, icon: 'link', label: 'Square Outline' },
  ];

  readonly showIconStyles = computed(
    () => this.customization().markerStyle === MarkerStyleType.Icon,
  );

  emitChange(patch: Partial<PersonalInformationCustomization>): void {
    this.customizationChange.emit({ ...this.customization(), ...patch });
  }
}
