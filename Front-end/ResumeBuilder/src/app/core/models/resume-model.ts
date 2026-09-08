export interface CreateResumeRequest {
  title: string;
  description: string | null;
}

export interface CreateResumeSectionRequest {
  resumeId: string;
  type: ResumeSectionType;
  displayOrder: number;
}

export interface CreateResumeResponse {
  id: string;
}

export interface CreateResumeSectionResponse {
  id: string;
}

export interface GenerateThumbnailResponse {
  thumbnailUrl: string;
}

export interface ResumeInternalResponse {
  id: string;
  title: string;
  description: string | null;
  status: ResumeStatus;
  settings: ResumeSettingInternalResponse;
  resumeSections: ResumeSectionInternalResponse[] | null;
  thumbnailUrl: string | null;
}

export enum ResumeStatus {
  Draft = 'Draft',
  Published = 'Published',
  Archived = 'Archived',
}

export interface ResumeSettingInternalResponse {
  column: number;
  font: string;
  fontSize: number;
  lineHeight: number;
  spaceBetweenElements: number;
  leftRightMargin: number;
  backgroundColorHeader: string;
  sectionTitleFontSize: number;
  sectionTitleColor: string;
  isPagenNumbersEnabled: boolean;
}

export interface ResumeSectionInternalResponse {
  id: string;
  type: ResumeSectionType;
  title: string;
  displayOrder: number;

  certificateEntries: CertificateEntryInternalResponse[] | null;
  educationEntries: EducationEntryInternalResponse[] | null;
  objectiveEntries: ObjectiveEntryInternalResponse[] | null;
  experienceEntries: ExperienceEntryInternalResponse[] | null;
  projectEntries: ProjectEntryInternalResponse[] | null;
  languageEntries: LanguageEntryInternalResponse[] | null;
  personalInformationEntries: PersonalInformationEntryInternalResponse[] | null;
  summaryEntries: SummaryEntryInternalResponse[] | null;
  skillEntries: SkillEntryInternalResponse[] | null;
}

export enum ResumeSectionType {
  PersonalInformation = 'PersonalInformation',
  Summary = 'Summary',
  Education = 'Education',
  Experience = 'Experience',
  Skills = 'Skills',
  Projects = 'Projects',
  Certificates = 'Certificates',
  Languages = 'Languages',
  Interests = 'Interests',
  Courses = 'Courses',
  Awards = 'Awards',
  Organizations = 'Organizations',
  Publications = 'Publications',
  References = 'References',
  Declaration = 'Declaration',
  Objective = 'Objective',
  Achievements = 'Achievements',
  Custom = 'Custom',
}

export enum LayoutType {
  Grid = 'Grid',
  Rows = 'Rows',
  Compact = 'Compact',
  Bubble = 'Bubble',
}

export enum TextAlignmentType {
  Start = 'Start',
  Center = 'Center',
  End = 'End',
}

export enum MarkerStyleType {
  None = 'None',
  Icon = 'Icon',
  Bullet = 'Bullet',
  Bar = 'Bar',
}

export enum IconStyleType {
  None = 'None',
  Default = 'Default',
  CircleFilled = 'CircleFilled',
  RoundedFilled = 'RoundedFilled',
  SquareFilled = 'SquareFilled',
  CircleOutline = 'CircleOutline',
  RoundedOutline = 'RoundedOutline',
  SquareOutline = 'SquareOutline',
}

export interface CertificateEntryInternalResponse {
  id: string;
  title: string | null;
  certificateUrl: string | null;
  description: string | null;
  displayOrder: number;
  certificateLayout: LayoutType;
  certificateGridColumn: number | null;
  certificateRowSpacing: string | null;
  isStartRowsWithBullet: boolean;
  subinfoStyle: string | null;
}

export interface EducationEntryInternalResponse {
  id: string;
  schoolName: string | null;
  degree: string | null;
  major: string | null;
  gpa: number | null;
  isCurrent: boolean;
  startDate: string | null;
  endDate: string | null;
  location: string | null;
  description: string | null;
  displayOrder: number;
  isByOrder: boolean;
}

export interface ObjectiveEntryInternalResponse {
  id: string;
  title: string | null;
  subTitle: string | null;
  description: string | null;
  displayOrder: number;
}

export interface ExperienceEntryInternalResponse {
  id: string;
  companyName: string | null;
  position: string | null;
  startDate: string | null;
  endDate: string | null;
  isCurrent: boolean;
  location: string | null;
  description: string | null;
  displayOrder: number;
  isByOrder: boolean;
}

export interface ProjectEntryInternalResponse {
  id: string;
  title: string | null;
  subTitle: string | null;
  startDate: string | null;
  endDate: string | null;
  projectUrl: string | null;
  repositoryUrl: string | null;
  description: string | null;
  displayOrder: number;
}

export interface LanguageEntryInternalResponse {
  id: string;
  languageName: string | null;
  proficiency: string | null;
  displayOrder: number;
}

export interface PersonalInformationEntryInternalResponse {
  id: string;
  fullName: string;
  professionalTitle: string;
  email: string | null;
  phoneNumber: string | null;
  address: string | null;
  website: string | null;
  linkedin: string | null;
  github: string | null;
  photoUrl: string | null;
  personalInformationTextAlignment: TextAlignmentType;
  personalInformationLayout: LayoutType;
  personalInformationMarkerStyle: MarkerStyleType;
  personalInformationIconStyle: IconStyleType;
  isTitleProfessionalTitleOnSameLine: boolean | null;
}

export interface SummaryEntryInternalResponse {
  id: string;
  summary: string | null;
  displayOrder: number;
}

export interface SkillEntryInternalResponse {
  id: string;
  skillName: string | null;
  description: string | null;
  skillLevel: string | null;
  displayOrder: number;
}

// ---- Create/Update request shapes, mirroring EntryController's Create*EntryCommand / Update*EntryRequest ----

export interface CreateEducationEntryRequest {
  resumeSectionId: string;
  schoolName: string | null;
  degree: string | null;
  major: string | null;
  gpa: number | null;
  isCurrent: boolean;
  startDate: string | null;
  endDate: string | null;
  location: string | null;
  description: string | null;
  isByOrder: boolean;
}

export interface UpdateEducationEntryRequest {
  schoolName: string | null;
  degree: string | null;
  major: string | null;
  gpa: number | null;
  isCurrent: boolean;
  startDate: string | null;
  endDate: string | null;
  location: string | null;
  description: string | null;
  isByOrder: boolean;
}

export interface UpdateEducationEntryStyleRequest {
  isByOrder: boolean;
}

export interface CreateExperienceEntryRequest {
  resumeSectionId: string;
  companyName: string | null;
  position: string | null;
  startDate: string | null;
  endDate: string | null;
  isCurrent: boolean;
  location: string | null;
  description: string | null;
  isByOrder: boolean;
}

export interface UpdateExperienceEntryRequest {
  companyName: string | null;
  position: string | null;
  startDate: string | null;
  endDate: string | null;
  isCurrent: boolean;
  location: string | null;
  description: string | null;
  isByOrder: boolean;
}

export interface CreateProjectEntryRequest {
  resumeSectionId: string;
  title: string | null;
  subTitle: string | null;
  startDate: string | null;
  endDate: string | null;
  projectUrl: string | null;
  repositoryUrl: string | null;
  description: string | null;
}

export interface UpdateProjectEntryRequest {
  title: string | null;
  subTitle: string | null;
  startDate: string | null;
  endDate: string | null;
  projectUrl: string | null;
  repositoryUrl: string | null;
  description: string | null;
}

export interface CreateCertificateEntryRequest {
  resumeSectionId: string;
  title: string | null;
  certificateUrl: string | null;
  description: string | null;
  certificateLayout: LayoutType;
  certificateGridColumn: number | null;
  certificateRowSpacing: string | null;
  isStartRowsWithBullet: boolean;
  subinfoStyle: string | null;
}

export interface UpdateCertificateEntryRequest {
  title: string | null;
  certificateUrl: string | null;
  description: string | null;
}

export interface UpdateCertificateEntryStyleRequest {
  certificateLayout: LayoutType;
  certificateGridColumn: number | null;
  certificateRowSpacing: string | null;
  isStartRowsWithBullet: boolean;
  subinfoStyle: string | null;
}

export interface CreateLanguageEntryRequest {
  resumeSectionId: string;
  languageName: string | null;
  proficiency: string | null;
  languageLayout: LayoutType;
  languageGridColumn: number | null;
  languageRowSpacing: string | null;
  isStartRowsWithBullet: boolean;
  subinfoStyle: string | null;
}

export interface UpdateLanguageEntryRequest {
  languageName: string | null;
  proficiency: string | null;
}

export interface CreateSkillEntryRequest {
  resumeSectionId: string;
  skillName: string | null;
  description: string | null;
  skillLevel: string | null;
  skillLayout: LayoutType;
  skillGridColumn: number | null;
  skillRowSpacing: string | null;
  isStartRowsWithBullet: boolean;
  subinfoStyle: string | null;
}

export interface UpdateSkillEntryRequest {
  skillName: string | null;
  description: string | null;
  skillLevel: string | null;
}

export interface CreateObjectiveEntryRequest {
  resumeSectionId: string;
  title: string | null;
  subTitle: string | null;
  description: string | null;
}

export interface UpdateObjectiveEntryRequest {
  title: string | null;
  subTitle: string | null;
  description: string | null;
}

export interface CreateSummaryEntryRequest {
  resumeSectionId: string;
  summary: string | null;
}

export interface UpdateSummaryEntryRequest {
  summary: string | null;
}

export interface UpdatePersonalInformationEntryRequest {
  fullName: string;
  professionalTitle: string;
  email: string | null;
  phoneNumber: string | null;
  address: string | null;
  website: string | null;
  linkedIn: string | null;
  gitHub: string | null;
  photoUrl: string | null;
}

export interface UpdatePersonalInformationEntryStyleRequest {
  personalInformationTextAlignment?: TextAlignmentType;
  personalInformationLayout?: LayoutType;
  personalInformationMarkerStyle?: MarkerStyleType;
  personalInformationIconStyle?: IconStyleType;
  isTitleProfessionalTitleOnSameLine?: boolean;
}

export interface UploadPersonalInformationPhotoResponse {
  photoUrl: string;
}
