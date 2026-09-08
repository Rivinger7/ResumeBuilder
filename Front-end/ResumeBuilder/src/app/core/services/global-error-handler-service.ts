import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { toast } from '@spartan-ng/brain/sonner';
import { HlmDialogService } from '@spartan-ng/helm/dialog';
import { ApiError } from '../errors/api-error';
import { ErrorDialogComponent } from '../../shared/components/error-dialog/error-dialog';

@Injectable({ providedIn: 'root' })
export class GlobalErrorHandlerService {
  private readonly router = inject(Router);
  private readonly dialogService = inject(HlmDialogService);

  /** True while an error dialog is open, to avoid stacking multiple dialogs (e.g. several 401s at once) */
  private isDialogOpen = false;

  handle(error: unknown): void {
    if (!(error instanceof ApiError)) {
      // Unknown runtime error (code bug, parse error, etc.) -> still treat as severe
      this.showDialog({
        title: 'Đã có lỗi xảy ra',
        message: 'Vui lòng thử lại. Nếu vẫn còn lỗi, hãy liên hệ đội hỗ trợ.',
      });
      console.error('[Unhandled error]', error);
      return;
    }

    // Let a component opt into silent handling (options.silent = true) to show the error inline itself
    if ((error as any).__silent) {
      return;
    }

    if (error.kind === 'unauthorized') {
      this.handleUnauthorized(error);
      return;
    }

    if (error.severity === 'dialog') {
      this.showDialog({ title: this.titleFor(error), message: error.message });
    } else {
      this.showToast(error);
    }
  }

  private handleUnauthorized(error: ApiError): void {
    this.showDialog({
      title: 'Phiên đăng nhập đã hết hạn',
      message: 'Vui lòng đăng nhập lại để tiếp tục.',
      onClose: () => this.router.navigateByUrl('/login'),
    });
  }

  private showToast(error: ApiError): void {
    switch (error.kind) {
      case 'validation':
        // Field errors exist -> generic toast, let the form show per-field errors itself
        toast.warning(error.message || 'Vui lòng kiểm tra lại thông tin đã nhập');
        break;
      case 'not-found':
        toast.info(error.message);
        break;
      case 'conflict':
      case 'bad-request':
        toast.error(error.message);
        break;
      default:
        toast.error(error.message);
    }
  }

  private showDialog(params: { title: string; message: string; onClose?: () => void }): void {
    if (this.isDialogOpen) return;
    this.isDialogOpen = true;

    const ref = this.dialogService.open(ErrorDialogComponent, {
      context: { title: params.title, message: params.message },
    });

    ref.closed$.subscribe(() => {
      this.isDialogOpen = false;
      params.onClose?.();
    });
  }

  private titleFor(error: ApiError): string {
    switch (error.kind) {
      case 'forbidden':
        return 'Không có quyền truy cập';
      case 'server-error':
        return 'Lỗi hệ thống';
      case 'network-error':
        return 'Mất kết nối';
      default:
        return 'Đã có lỗi xảy ra';
    }
  }
}
