import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { HlmSidebarImports } from '@spartan-ng/helm/sidebar';
import { HlmDropdownMenuImports } from '@spartan-ng/helm/dropdown-menu';
import { LucideIconModule } from '../../../../shared/utils/lucide-icon-module';
import { SessionFacade } from '../../../facade/session-facade';
import { AuthDialogService } from '../../../../features/authentication/auth-dialog/auth-dialog-service';

type NavItem = {
  label: string;
  route: string;
  icon: string;
};

@Component({
  selector: 'app-sidebar',
  imports: [
    RouterLink,
    RouterLinkActive,
    LucideIconModule,
    HlmSidebarImports,
    HlmDropdownMenuImports,
  ],
  templateUrl: './sidebar.html',
})
export class Sidebar {
  readonly HouseIcon = 'house';
  readonly MailIcon = 'mail';
  readonly ChevronUpIcon = 'chevron-up';
  readonly SparklesIcon = 'sparkles';
  readonly CircleUserIcon = 'circle-user';
  readonly CreditCardIcon = 'credit-card';
  readonly BellIcon = 'bell';
  readonly LogOutIcon = 'log-out';
  readonly UserKeyIcon = 'user-key';
  readonly LogInIcon = 'log-in';

  private readonly session = inject(SessionFacade);
  private readonly router = inject(Router);
  private readonly authDialog = inject(AuthDialogService);

  readonly userProfile = this.session.userProfile;

  mainNavItems: NavItem[] = [
    { label: 'Home', icon: this.HouseIcon, route: '/home' },
    { label: 'Cover Letter', icon: this.MailIcon, route: '/cover-letter' },
    // { label: 'Job Tracker', icon: '', route: '/job-tracker' },
  ];

  bottomLinks: NavItem[] = [
    { label: 'Plans & Pricing', icon: '', route: '/pricing' },
    { label: 'Student Benefits', icon: '', route: '/student-benefits' },
  ];

  openLoginDialog() {
    this.authDialog.openLoginDialog();
  }

  openRegisterDialog() {
    this.authDialog.openRegisterDialog();
  }

  async logout() {
    await this.session.logout();
    this.router.navigate(['/home']);
  }
}
