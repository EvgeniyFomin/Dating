import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { HasRoleDirective } from '../_directives/has-role.directive';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule, BsDropdownModule, RouterLink, RouterLinkActive, HasRoleDirective],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})

export class NavComponent {
  accountService = inject(AccountService);
  private router = inject(Router);
  private toastr = inject(ToastrService);
  model: any = {};

  login() {
    this.accountService.login(this.model).subscribe({
      next: _ => {
        if (this.accountService.roles()?.some((r: string) => r?.toLowerCase() === 'admin')) {
          this.router.navigateByUrl('/admin')
        }
        else {
          this.router.navigateByUrl('/members')
        }
      },
      error: error => {
        const errors: string[] = error;
        errors.forEach(item => {
          this.toastr.error(item);
        });
      }
    })
  };

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/home');
  }
}
