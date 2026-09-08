import {
  ApplicationConfig,
  importProvidersFrom,
  inject,
  provideAppInitializer,
  provideBrowserGlobalErrorListeners,
} from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { SessionFacade } from './core/facade/session-facade';
import { LucideIconModule } from './shared/utils/lucide-icon-module';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),

    // Dialogs opened via CDK's Dialog.open() (AddContentDialog, AuthDialog,
    // CreateResumeDialog...) are instantiated outside the normal component tree, so a
    // component-level `imports: [LucideIconModule]` doesn't reliably supply their icon
    // registry — lucide-icon throws "not provided by any icon providers" inside them.
    // Registering the icons on the root injector guarantees every dynamically-created
    // component can resolve them too.
    importProvidersFrom(LucideIconModule),

    provideAppInitializer(async () => {
      const session = inject(SessionFacade);
      await session.restoreSession();
    }),
  ],
};
