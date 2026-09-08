import { Injectable, computed, inject, signal } from '@angular/core';
import { ApiClientService } from '../../services/api-client-service';
import { GlobalErrorHandlerService } from '../../services/global-error-handler-service';
import { ProfileResponse } from '../../models/user-model';

@Injectable({ providedIn: 'root' })
export class UserService {
  private readonly api = inject(ApiClientService);
  private readonly errorHandler = inject(GlobalErrorHandlerService);

  private readonly endpoint = '/users';

  readonly userProfile = signal<ProfileResponse | undefined>(undefined);

  async loadUserProfile(): Promise<void> {
    try {
      const data = await this.api.get<ProfileResponse>(`${this.endpoint}/profile/me`);
      this.userProfile.set(data);
    } catch (error) {
      this.errorHandler.handle(error);
      throw error;
    }
  }

  clearProfile(): void {
    this.userProfile.set(undefined);
  }
}
