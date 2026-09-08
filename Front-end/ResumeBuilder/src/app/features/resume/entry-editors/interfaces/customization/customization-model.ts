import {
  IconStyleType,
  LayoutType,
  MarkerStyleType,
  TextAlignmentType,
} from '../../../../../core/models/resume-model';

export interface PersonalInformationCustomization {
  textAlignment: TextAlignmentType;
  layout: LayoutType;
  markerStyle: MarkerStyleType;
  iconStyle: IconStyleType;
}

export interface EducationCustomization {
  isByOrder: boolean;
}

export interface CertificationCustomization {
  certificateLayout: LayoutType;
  certificateGridColumn: number | null;
  certificateRowSpacing: string | null;
  isStartRowsWithBullet: boolean;
  subinfoStyle: string | null;
}
