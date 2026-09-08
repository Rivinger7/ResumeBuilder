import { Component, computed, input } from '@angular/core';
import {
  IconStyleType,
  LayoutType,
  MarkerStyleType,
  PersonalInformationEntryInternalResponse,
} from '../../../../../core/models/resume-model';
import { NAVY_COLOR } from '../../../../../shared/models/color';
import { NgStyle, NgClass } from '@angular/common';
import { LucideIconModule } from '../../../../../shared/utils/lucide-icon-module';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faGithub, faLinkedin } from '@fortawesome/free-brands-svg-icons';

@Component({
  selector: 'app-personal-information-seciton-preview',
  imports: [NgStyle, NgClass, LucideIconModule, FontAwesomeModule],
  templateUrl: './personal-information-seciton-preview.html',
})
export class PersonalInformationSecitonPreview {
  faGithub = faGithub;
  faLinkedin = faLinkedin;

  personalInfo = input.required<PersonalInformationEntryInternalResponse | undefined>();
  personalInfoSettings = input.required<string | undefined>();

  readonly headerStyle = computed(() => {
    return {
      'align-items': this.personalInfo()?.personalInformationTextAlignment ?? 'center',
      'background-color': this.personalInfoSettings() ?? NAVY_COLOR,
    };
  });

  readonly contactClass = computed(() => {
    switch (this.personalInfo()?.personalInformationLayout) {
      case LayoutType.Grid:
        return 'grid grid-cols-2 gap-2';

      case LayoutType.Rows:
        return 'flex flex-row flex-wrap gap-2';

      case LayoutType.Compact:
        return 'flex flex-row flex-nowrap gap-2';

      case LayoutType.Bubble:
        return 'flex flex-row flex-wrap gap-2';

      default:
        return 'gap-2';
    }
  });

  readonly contacts = computed(() => {
    const personalInfo = this.personalInfo();

    if (!personalInfo) {
      return [];
    }

    return [
      {
        type: 'email',
        value: personalInfo.email,
        icon: 'mail',
      },
      {
        type: 'phone',
        value: personalInfo.phoneNumber,
        icon: 'phone',
      },
      {
        type: 'address',
        value: personalInfo.address,
        icon: 'map-pin',
      },
      {
        type: 'website',
        value: personalInfo.website,
        icon: 'globe',
      },
      {
        type: 'linkedin',
        value: personalInfo.linkedin,
        icon: 'linkedin',
      },
      {
        type: 'github',
        value: personalInfo.github,
        icon: 'github',
      },
    ].filter((x) => !!x.value?.trim());
  });

  readonly isVisibleContactIcons = computed(
    () =>
      this.personalInfo()?.personalInformationMarkerStyle === MarkerStyleType.Icon &&
      this.personalInfo()?.personalInformationIconStyle !== IconStyleType.None,
  );

  readonly contactIconClass = computed(() => {
    switch (this.personalInfo()?.personalInformationIconStyle) {
      case IconStyleType.Default:
        return 'inline-flex items-center justify-center';

      case IconStyleType.CircleFilled:
        return 'inline-flex items-center justify-center rounded-full bg-white/20 p-1';

      case IconStyleType.RoundedFilled:
        return 'inline-flex items-center justify-center rounded-md bg-white/20 p-1';

      case IconStyleType.SquareFilled:
        return 'inline-flex items-center justify-center bg-white/20 p-1';

      case IconStyleType.CircleOutline:
        return 'inline-flex items-center justify-center rounded-full border border-white/50 p-1';

      case IconStyleType.RoundedOutline:
        return 'inline-flex items-center justify-center rounded-md border border-white/50 p-1';

      case IconStyleType.SquareOutline:
        return 'inline-flex items-center justify-center border border-white/50 p-1';

      case IconStyleType.None:
      default:
        return '';
    }
  });

  readonly contactMarker = computed(() => {
    const marker = this.personalInfo()?.personalInformationMarkerStyle;

    switch (marker) {
      case MarkerStyleType.Bar:
        return '|';

      case MarkerStyleType.Bullet:
        return '•';

      case MarkerStyleType.None:
      default:
        return '';
    }
  });
}
