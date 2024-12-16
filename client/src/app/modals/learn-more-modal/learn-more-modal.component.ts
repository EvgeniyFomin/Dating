import { Component, inject } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-learn-more-modal',
  standalone: true,
  imports: [],
  templateUrl: './learn-more-modal.component.html',
  styleUrl: './learn-more-modal.component.css'
})
export class LearnMoreModalComponent {

  modalRef = inject(BsModalRef);

}
