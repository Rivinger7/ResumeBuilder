import { Injectable, inject } from '@angular/core';
import { ApiClientService } from '../api-client-service';
import { GlobalErrorHandlerService } from '../global-error-handler-service';
import {
  CreateCertificateEntryRequest,
  CreateEducationEntryRequest,
  CreateExperienceEntryRequest,
  CreateLanguageEntryRequest,
  CreateObjectiveEntryRequest,
  CreateProjectEntryRequest,
  CreateSkillEntryRequest,
  CreateSummaryEntryRequest,
  ResumeSectionType,
  UpdateCertificateEntryRequest,
  UpdateCertificateEntryStyleRequest,
  UpdateEducationEntryRequest,
  UpdateEducationEntryStyleRequest,
  UpdateExperienceEntryRequest,
  UpdateLanguageEntryRequest,
  UpdateObjectiveEntryRequest,
  UpdatePersonalInformationEntryRequest,
  UpdatePersonalInformationEntryStyleRequest,
  UpdateProjectEntryRequest,
  UpdateSkillEntryRequest,
  UpdateSummaryEntryRequest,
  UploadPersonalInformationPhotoResponse,
} from '../../models/resume-model';

@Injectable({
  providedIn: 'root',
})
export class EntryService {
  private readonly api = inject(ApiClientService);
  private readonly errorHandler = inject(GlobalErrorHandlerService);

  private readonly endpoint = '/entries';
  private readonly displayOrderEndpoint = '/display-orders';

  private async withErrorHandling<T>(fn: () => Promise<T>): Promise<T> {
    try {
      return await fn();
    } catch (error) {
      this.errorHandler.handle(error);
      throw error;
    }
  }

  // ---- Education ----
  async createEducation(request: CreateEducationEntryRequest): Promise<void> {
    await this.withErrorHandling(() => this.api.post(`${this.endpoint}/educations`, request));
  }

  async updateEducation(id: string, request: Partial<UpdateEducationEntryRequest>): Promise<void> {
    await this.withErrorHandling(() =>
      this.api.patch(`${this.endpoint}/educations/${id}`, request),
    );
  }

  async updateEducationStyle(id: string, request: UpdateEducationEntryStyleRequest): Promise<void> {
    await this.withErrorHandling(() =>
      this.api.patch(`${this.endpoint}/educations/${id}`, request),
    );
  }

  async deleteEducation(id: string): Promise<void> {
    await this.withErrorHandling(() => this.api.delete(`${this.endpoint}/educations/${id}`));
  }

  // ---- Experience ----
  async createExperience(request: CreateExperienceEntryRequest): Promise<void> {
    await this.withErrorHandling(() => this.api.post(`${this.endpoint}/experiences`, request));
  }

  async updateExperience(
    id: string,
    request: Partial<UpdateExperienceEntryRequest>,
  ): Promise<void> {
    await this.withErrorHandling(() =>
      this.api.patch(`${this.endpoint}/experiences/${id}`, request),
    );
  }

  async deleteExperience(id: string): Promise<void> {
    await this.withErrorHandling(() => this.api.delete(`${this.endpoint}/experiences/${id}`));
  }

  // ---- Projects ----
  async createProject(request: CreateProjectEntryRequest): Promise<void> {
    await this.withErrorHandling(() => this.api.post(`${this.endpoint}/projects`, request));
  }

  async updateProject(id: string, request: Partial<UpdateProjectEntryRequest>): Promise<void> {
    await this.withErrorHandling(() => this.api.patch(`${this.endpoint}/projects/${id}`, request));
  }

  async deleteProject(id: string): Promise<void> {
    await this.withErrorHandling(() => this.api.delete(`${this.endpoint}/projects/${id}`));
  }

  // ---- Certificates ----
  async createCertificate(request: CreateCertificateEntryRequest): Promise<void> {
    await this.withErrorHandling(() => this.api.post(`${this.endpoint}/certificates`, request));
  }

  async updateCertificate(
    id: string,
    request: Partial<UpdateCertificateEntryRequest>,
  ): Promise<void> {
    await this.withErrorHandling(() =>
      this.api.patch(`${this.endpoint}/certificates/${id}`, request),
    );
  }

  async updateCertificateStyle(
    id: string,
    request: UpdateCertificateEntryStyleRequest,
  ): Promise<void> {
    await this.withErrorHandling(() =>
      this.api.patch(`${this.endpoint}/certificates/${id}/style`, request),
    );
  }

  async deleteCertificate(id: string): Promise<void> {
    await this.withErrorHandling(() => this.api.delete(`${this.endpoint}/certificates/${id}`));
  }

  // ---- Languages ----
  async createLanguage(request: CreateLanguageEntryRequest): Promise<void> {
    await this.withErrorHandling(() => this.api.post(`${this.endpoint}/languages`, request));
  }

  async updateLanguage(id: string, request: Partial<UpdateLanguageEntryRequest>): Promise<void> {
    await this.withErrorHandling(() => this.api.patch(`${this.endpoint}/languages/${id}`, request));
  }

  async deleteLanguage(id: string): Promise<void> {
    await this.withErrorHandling(() => this.api.delete(`${this.endpoint}/languages/${id}`));
  }

  // ---- Skills ----
  async createSkill(request: CreateSkillEntryRequest): Promise<void> {
    await this.withErrorHandling(() => this.api.post(`${this.endpoint}/skills`, request));
  }

  async updateSkill(id: string, request: Partial<UpdateSkillEntryRequest>): Promise<void> {
    await this.withErrorHandling(() => this.api.patch(`${this.endpoint}/skills/${id}`, request));
  }

  async deleteSkill(id: string): Promise<void> {
    await this.withErrorHandling(() => this.api.delete(`${this.endpoint}/skills/${id}`));
  }

  // ---- Objectives ----
  async createObjective(request: CreateObjectiveEntryRequest): Promise<void> {
    await this.withErrorHandling(() => this.api.post(`${this.endpoint}/objectives`, request));
  }

  async updateObjective(id: string, request: Partial<UpdateObjectiveEntryRequest>): Promise<void> {
    await this.withErrorHandling(() =>
      this.api.patch(`${this.endpoint}/objectives/${id}`, request),
    );
  }

  async deleteObjective(id: string): Promise<void> {
    await this.withErrorHandling(() => this.api.delete(`${this.endpoint}/objectives/${id}`));
  }

  // ---- Summaries ----
  async createSummary(request: CreateSummaryEntryRequest): Promise<void> {
    await this.withErrorHandling(() => this.api.post(`${this.endpoint}/summaries`, request));
  }

  async updateSummary(id: string, request: UpdateSummaryEntryRequest): Promise<void> {
    // BE uses PUT for summaries, unlike PATCH for other entry types.
    await this.withErrorHandling(() => this.api.put(`${this.endpoint}/summaries/${id}`, request));
  }

  async deleteSummary(id: string): Promise<void> {
    await this.withErrorHandling(() => this.api.delete(`${this.endpoint}/summaries/${id}`));
  }

  // ---- Personal information (update-only, auto-created with the resume) ----
  async updatePersonalInformation(
    id: string,
    request: Partial<UpdatePersonalInformationEntryRequest>,
  ): Promise<void> {
    await this.withErrorHandling(() =>
      this.api.patch(`${this.endpoint}/personal-information/${id}`, request),
    );
  }

  async updatePersonalInformationStyle(
    id: string,
    request: UpdatePersonalInformationEntryStyleRequest,
  ): Promise<void> {
    await this.withErrorHandling(() =>
      this.api.patch(`${this.endpoint}/personal-information/${id}/style`, request),
    );
  }

  async uploadPersonalInformationPhoto(
    id: string,
    file: File,
  ): Promise<UploadPersonalInformationPhotoResponse> {
    const formData = new FormData();
    formData.append('file', file);
    return this.withErrorHandling(() =>
      this.api.post<UploadPersonalInformationPhotoResponse>(
        `${this.endpoint}/personal-information/${id}/photo`,
        formData,
      ),
    );
  }

  // ---- Reorder ----
  async reorderEntry(
    resumeSectionId: string,
    id: string,
    type: ResumeSectionType,
    newDisplayOrder: number,
  ): Promise<void> {
    await this.withErrorHandling(() =>
      this.api.patch(
        `${this.displayOrderEndpoint}/resume-sections/${resumeSectionId}/entries/${id}`,
        { type, newDisplayOrder },
      ),
    );
  }
}
