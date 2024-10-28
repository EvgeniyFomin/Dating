import { TimeagoModule } from 'ngx-timeago';
import { Component, inject, input, output, ViewChild } from '@angular/core';
import { MessagesService } from '../../_services/messages.service';
import { Message } from '../../_models/message';
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
  messages = input.required<Message[]>();
  messageContent = '';
  updateMessages = output<Message>();

  sendMessage() {
    this.messagesService.sendMessage(this.userId(), this.messageContent).subscribe({
      next: message => {
        this.updateMessages.emit(message);
        this.messageForm?.reset();
      }
    })
  }
}
