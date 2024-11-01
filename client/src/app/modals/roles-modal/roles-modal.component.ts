import { Component, TemplateRef, inject } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-roles-modal',
  standalone: true,
  imports: [],
  templateUrl: './roles-modal.component.html',
  styleUrl: './roles-modal.component.css'
})
export class RolesModalComponent {
  modalRef = inject(BsModalRef);
  title = '';
  list: string[] = [];

  constructor(private modalService: BsModalService) { }

  openModal(template: TemplateRef<void>) {
    this.modalRef = this.modalService.show(template);
  }

}
