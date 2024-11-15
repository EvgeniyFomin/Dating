import { CanDeactivateFn } from '@angular/router';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';
import { inject } from '@angular/core';
import { ConfirmService } from '../_services/confirm.service';

export const preventUnsavedChangesGuard: CanDeactivateFn<MemberEditComponent> = (component) => {
  const confirmServie = inject(ConfirmService)
  if (component.editForm?.dirty) {
    return confirmServie.confirm() ?? false;
  }

  return true;
};
