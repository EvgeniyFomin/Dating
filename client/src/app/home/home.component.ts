import { Component, inject } from '@angular/core';
import { RegisterComponent } from "../register/register.component";
import { NgIf } from '@angular/common';
import { AccountService } from '../_services/account.service';
import { LearnMoreModalComponent } from '../modals/learn-more-modal/learn-more-modal.component';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterComponent, NgIf],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})

export class HomeComponent {
  accountService = inject(AccountService);
  modalService = inject(BsModalService);
  bsModalRef?: BsModalRef<LearnMoreModalComponent> = new BsModalRef<LearnMoreModalComponent>;
  isRegisterMode = false;

  registerToggle() {
    this.isRegisterMode = !this.isRegisterMode;
  }
  cancelRegisterMode(event: boolean) {
    this.isRegisterMode = event;
  }

  showLearnMore() {
    this.bsModalRef = this.modalService.show(LearnMoreModalComponent);
  }
}
