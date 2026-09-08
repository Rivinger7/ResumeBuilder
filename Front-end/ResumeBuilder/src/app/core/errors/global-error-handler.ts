import { ErrorHandler, Injectable, Injector, inject } from '@angular/core';
import { GlobalErrorHandlerService } from '../services/global-error-handler-service';

/**
 * Implements Angular's ErrorHandler, registered in app.config.ts. Uses Injector instead
 * of inject() directly because ErrorHandler is instantiated earlier than some other
 * providers in the DI graph.
 */
@Injectable()
export class AppGlobalErrorHandler implements ErrorHandler {
  private readonly injector = inject(Injector);

  handleError(error: unknown): void {
    const service = this.injector.get(GlobalErrorHandlerService);
    // Angular sometimes wraps async errors in { rejection } -> unwrap
    const actual = (error as any)?.rejection ?? error;
    service.handle(actual);
  }
}
