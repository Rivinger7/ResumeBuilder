import { Injectable, Injector, inject } from '@angular/core';
import axios, { AxiosInstance, AxiosRequestConfig, InternalAxiosRequestConfig } from 'axios';
import { ApiProblemDetails, ApiSuccessResponse } from '../models/api-response';
import { ApiError } from '../errors/api-error';
import { AuthTokenService } from './auth-token-service';
import { SessionFacade } from '../facade/session-facade';

export interface RequestOptions extends AxiosRequestConfig {
  /** Skip the global error handler (dialog/toast) for this request; caller handles it */
  silent?: boolean;
  _retry?: boolean;
}

@Injectable({ providedIn: 'root' })
export class ApiClientService {
  private readonly authToken = inject(AuthTokenService);
  private readonly injector = inject(Injector);
  private readonly http: AxiosInstance;
  private readonly baseUrl = import.meta.env['NG_APP_API_URL'];

  /** BE origin — used to build absolute URLs for <img src>/<a href> pointing at
   * BE-served files (thumbnails, ...), since those are native browser requests that
   * don't go through the axios baseURL. */
  readonly origin = new URL(this.baseUrl).origin;

  constructor() {
    this.http = axios.create({
      baseURL: this.baseUrl,
      withCredentials: true,
    });

    this.setupInterceptors();
  }

  get<T>(path: string, options?: RequestOptions): Promise<T> {
    return this.request<T>({ ...options, method: 'GET', url: path });
  }

  post<T>(path: string, body?: unknown, options?: RequestOptions): Promise<T> {
    return this.request<T>({ ...options, method: 'POST', url: path, data: body });
  }

  put<T>(path: string, body?: unknown, options?: RequestOptions): Promise<T> {
    return this.request<T>({ ...options, method: 'PUT', url: path, data: body });
  }

  patch<T>(path: string, body?: unknown, options?: RequestOptions): Promise<T> {
    return this.request<T>({ ...options, method: 'PATCH', url: path, data: body });
  }

  delete<T>(path: string, options?: RequestOptions): Promise<T> {
    return this.request<T>({ ...options, method: 'DELETE', url: path });
  }

  // Binary response (e.g. a PDF file) — doesn't unwrap ApiSuccessResponse<T> like request() does.
  async getBlob(path: string, options?: RequestOptions): Promise<Blob> {
    const response = await this.http.request<Blob>({
      ...options,
      method: 'GET',
      url: path,
      responseType: 'blob',
    });
    return response.data;
  }

  private async request<T>(config: RequestOptions): Promise<T> {
    const response = await this.http.request<ApiSuccessResponse<T>>(config);

    // 204 No Content - axios returns data = '' or undefined depending on the server
    if (response.status === 204 || !response.data) {
      return undefined as T;
    }

    return response.data.data;
  }

  private setupInterceptors(): void {
    this.http.interceptors.request.use((config: InternalAxiosRequestConfig) => {
      const token = this.authToken.get();
      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }
      return config;
    });

    this.http.interceptors.response.use(
      (response) => response,
      async (error) => {
        const originalRequest = error.config as RequestOptions | undefined;
        const silent = originalRequest?.silent;

        if (!error.response) {
          const err = ApiError.networkError(error);
          (err as any).__silent = silent;
          return Promise.reject(err);
        }

        const status = error.response.status;
        // /auth/refresh and /auth/logout must not go through the refresh+logout retry —
        // otherwise a 401 on logout would retry into logout again, looping forever.
        const isAuthEndpointCall =
          originalRequest?.url?.includes('/auth/refresh') ||
          originalRequest?.url?.includes('/auth/logout');

        if (status === 401 && originalRequest && !originalRequest._retry && !isAuthEndpointCall) {
          originalRequest._retry = true;
          const session = this.injector.get(SessionFacade);

          try {
            await session.refreshSession();
            return this.http.request(originalRequest);
          } catch {
            await session.logout();
            const err = ApiError.fromProblemDetails(status, error.response.data);
            return Promise.reject(err);
          }
        }

        const problem = error.response.data as ApiProblemDetails | undefined;
        const err = ApiError.fromProblemDetails(status, problem);
        (err as any).__silent = silent;
        return Promise.reject(err);
      },
    );
  }
}
