import { Routes } from '@angular/router';
import { ResumeStorage } from './features/homepage/resume-storage/resume-storage';
import { ResumeDetail } from './features/resume/resume-detail/resume-detail';

export const routes: Routes = [
  {
    path: '',
    component: ResumeStorage,
  },
  {
    path: 'home',
    component: ResumeStorage,
  },
  {
    path: 'resumes/:id/edit',
    component: ResumeDetail,
  },
];
