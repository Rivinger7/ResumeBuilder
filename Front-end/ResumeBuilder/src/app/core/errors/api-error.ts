import { ApiErrorKind, ApiProblemDetails, ErrorSeverity } from '../models/api-response';

/**
 * Single exception type thrown by ApiClient for every HTTP error (>=400) or network
 * error. The UI layer (toast/dialog) only needs to know this type, never Response/fetch.
 */
export class ApiError extends Error {
  readonly kind: ApiErrorKind;
  readonly severity: ErrorSeverity;
  readonly status: number;
  readonly problem?: ApiProblemDetails;
  /** Per-field validation errors, for binding into a form (if any) */
  readonly fieldErrors?: Record<string, string[]>;

  constructor(params: {
    kind: ApiErrorKind;
    severity: ErrorSeverity;
    status: number;
    message: string;
    problem?: ApiProblemDetails;
    fieldErrors?: Record<string, string[]>;
  }) {
    super(params.message);
    this.name = 'ApiError';
    this.kind = params.kind;
    this.severity = params.severity;
    this.status = params.status;
    this.problem = params.problem;
    this.fieldErrors = params.fieldErrors;
  }

  /**
   * Maps status -> kind -> severity. The single place that decides toast vs dialog.
   */
  static fromProblemDetails(status: number, problem?: ApiProblemDetails): ApiError {
    switch (status) {
      case 400:
        if (problem?.errors) {
          return new ApiError({
            kind: 'validation',
            severity: 'toast', // field errors exist -> prefer inline per-field display, toast is just a nudge
            status,
            message: problem.detail ?? 'Dữ liệu không hợp lệ',
            problem,
            fieldErrors: problem.errors,
          });
        }
        return new ApiError({
          kind: 'bad-request',
          severity: 'toast',
          status,
          message: problem?.title ?? 'Yêu cầu không hợp lệ',
          problem,
        });

      case 401:
        return new ApiError({
          kind: 'unauthorized',
          severity: 'dialog',
          status,
          message: problem?.title ?? 'Phiên đăng nhập đã hết hạn',
          problem,
        });

      case 403:
        return new ApiError({
          kind: 'forbidden',
          severity: 'dialog',
          status,
          message: problem?.title ?? 'Bạn không có quyền thực hiện thao tác này',
          problem,
        });

      case 404:
        return new ApiError({
          kind: 'not-found',
          severity: 'toast',
          status,
          message: problem?.title ?? 'Không tìm thấy dữ liệu',
          problem,
        });

      case 409:
        return new ApiError({
          kind: 'conflict',
          severity: 'toast',
          status,
          message: problem?.title ?? 'Dữ liệu đã tồn tại hoặc bị xung đột',
          problem,
        });

      default:
        // 500 and any other unknown status -> generic message, never leak BE detail
        return new ApiError({
          kind: 'server-error',
          severity: 'dialog',
          status,
          message: 'Đã có lỗi xảy ra từ hệ thống. Vui lòng thử lại sau ít phút.',
          problem,
        });
    }
  }

  static networkError(cause?: unknown): ApiError {
    return new ApiError({
      kind: 'network-error',
      severity: 'dialog',
      status: 0,
      message: 'Không thể kết nối đến máy chủ. Vui lòng kiểm tra kết nối mạng.',
    });
  }
}
