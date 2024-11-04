import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

export const adminGuard: CanActivateFn = (route, state) => {
  const accountsService = inject(AccountService);
  const toastr = inject(ToastrService);

  if (accountsService.roles().includes('Admin') || accountsService.roles().includes('Moderator')) {
    return true;
  }
  else {
    toastr.error('You cannot get this area');
    return false;
  }

};
