import { TimeagoModule } from 'ngx-timeago';
import { Component, inject, input, ViewChild } from '@angular/core';
import { MessagesService } from '../../_services/messages.service';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-messages',
  standalone: true,
  imports: [TimeagoModule, FormsModule],
  templateUrl: './member-messages.component.html',
  styleUrl: './member-messages.component.css'
})

export class MemberMessagesComponent {
  @ViewChild('messageForm') messageForm?: NgForm;
  messagesService = inject(MessagesService);
  userId = input.required<number>();
  messageContent = '';

  sendMessage() {
    this.messagesService.sendMessage(this.userId(), this.messageContent).then(() => {
      this.messageForm?.reset();
    })
  }
}
