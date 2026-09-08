/**
 * Matches the anonymous object in ResponseMiddleware.cs (BE, camelCased by PropertyNamingPolicy)
 */
export interface ApiSuccessResponse<T = unknown> {
  statusCode: number;
  message: string;
  data: T;
  traceId: string;
  path: string;
  timestamp: string;
}

/**
 * Matches the RFC 7807 ProblemDetails returned by ASP.NET Core
 * (Status, Title, Type, Instance from ExceptionMiddleware.CreateProblem)
 */
export interface ApiProblemDetails {
  status: number;
  title: string;
  type: string;
  instance: string;
  detail?: string;
  /** Only present for ValidationProblemDetails (FluentValidation.ValidationException) */
  errors?: Record<string, string[]>;
}

export type ErrorSeverity = 'toast' | 'dialog';

/**
 * Business error categories, mapped 1:1 to Domain exceptions so the FE doesn't have
 * to guess from status codes alone.
 */
export type ApiErrorKind =
  | 'validation' // 400 - FluentValidation.ValidationException
  | 'bad-request' // 400 - BadRequestException
  | 'unauthorized' // 401 - UnauthorizedException
  | 'forbidden' // 403 - ForbiddenException
  | 'not-found' // 404 - NotFoundException
  | 'conflict' // 409 - ConflictException
  | 'server-error' // 500 - unhandled
  | 'network-error'; // Fetch failed with no response (offline, CORS, timeout...)
