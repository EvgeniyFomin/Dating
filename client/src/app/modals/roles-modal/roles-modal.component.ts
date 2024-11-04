import { Component, inject, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { User } from '../../_models/user';

@Component({
  selector: 'app-roles-modal',
  standalone: true,
  imports: [],
  templateUrl: './roles-modal.component.html',
  styleUrl: './roles-modal.component.css'
})
export class RolesModalComponent implements OnInit {
  ngOnInit(): void {
    this.currentRoles = this.user.roles.slice();
    this.selectedRoles = this.user.roles.slice();
  }
  user: User = {} as User;
  modalRef = inject(BsModalRef);
  title = '';
  currentRoles: string[] = [];
  availableRoles: string[] = [];
  selectedRoles: string[] = [];
  isRolesUpdated = false;

  updateChecked(checkedValue: string) {
    if (this.selectedRoles.includes(checkedValue)) {
      this.selectedRoles = this.selectedRoles.filter(r => r !== checkedValue)
    }
    else {
      this.selectedRoles.push(checkedValue);
    }
    this.isRolesUpdated = JSON.stringify(this.currentRoles.sort()) != JSON.stringify(this.selectedRoles.sort());
  }
}