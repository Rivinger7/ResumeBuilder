import { Injectable, inject, signal } from '@angular/core';
import { ApiClientService, RequestOptions } from '../api-client-service';
import {
  CreateResumeRequest,
  CreateResumeResponse,
  CreateResumeSectionRequest,
  CreateResumeSectionResponse,
  GenerateThumbnailResponse,
  ResumeInternalResponse,
} from '../../models/resume-model';
import { GlobalErrorHandlerService } from '../global-error-handler-service';
import { PagedResponse } from '../../models/pagination-model';

@Injectable({
  providedIn: 'root',
})
export class ResumeService {
  private readonly api = inject(ApiClientService);
  private readonly errorHandler = inject(GlobalErrorHandlerService);

  private readonly endpoint = '/resumes';

  readonly resumes = signal<ResumeInternalResponse[] | undefined>(undefined);

  private async withErrorHandling<T>(fn: () => Promise<T>): Promise<T> {
    try {
      return await fn();
    } catch (error) {
      this.errorHandler.handle(error);
      throw error;
    }
  }

  async loadAllResumes(): Promise<void> {
    const data = await this.withErrorHandling(() =>
      this.api.get<PagedResponse<ResumeInternalResponse>>(`${this.endpoint}/me`),
    );
    this.resumes.set(data.items);
  }

  async create(
    request: CreateResumeRequest,
    options?: RequestOptions,
  ): Promise<CreateResumeResponse> {
    return this.withErrorHandling(() =>
      this.api.post<CreateResumeResponse>(this.endpoint, request, options),
    );
  }

  async createResumeSection(
    request: CreateResumeSectionRequest,
  ): Promise<CreateResumeSectionResponse> {
    return this.withErrorHandling(() =>
      this.api.post<CreateResumeSectionResponse>(`${this.endpoint}/sections`, request),
    );
  }

  async deleteResumeSection(id: string): Promise<void> {
    await this.withErrorHandling(() =>
      this.api.delete<void>(`${this.endpoint}/sections/${encodeURIComponent(id)}`),
    );
  }

  async delete(id: string): Promise<void> {
    await this.withErrorHandling(() =>
      this.api.delete<void>(`${this.endpoint}/${encodeURIComponent(id)}`),
    );
  }

  async getById(id: string): Promise<ResumeInternalResponse> {
    return this.withErrorHandling(() =>
      this.api.get<ResumeInternalResponse>(`${this.endpoint}/${encodeURIComponent(id)}`),
    );
  }

  async generateThumbnail(id: string): Promise<GenerateThumbnailResponse> {
    return this.withErrorHandling(() =>
      this.api.post<GenerateThumbnailResponse>(
        `${this.endpoint}/${encodeURIComponent(id)}/generate-thumbnail`,
      ),
    );
  }

  async downloadPdf(id: string, fileName: string): Promise<void> {
    const blob = await this.withErrorHandling(() =>
      this.api.getBlob(`${this.endpoint}/${encodeURIComponent(id)}/export-pdf`),
    );

    const url = URL.createObjectURL(blob);
    const anchor = document.createElement('a');
    anchor.href = url;
    anchor.download = fileName;
    anchor.click();
    URL.revokeObjectURL(url);
  }

  async reorderSection(resumeId: string, id: string, newDisplayOrder: number): Promise<void> {
    await this.withErrorHandling(() =>
      this.api.patch(`/display-orders/resumes/${resumeId}/resume-sections/${id}`, {
        newDisplayOrder,
      }),
    );
  }
}
