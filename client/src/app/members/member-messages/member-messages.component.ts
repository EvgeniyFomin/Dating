import { TimeagoModule } from 'ngx-timeago';
import { AfterViewChecked, Component, inject, input, ViewChild } from '@angular/core';
import { MessagesService } from '../../_services/messages.service';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-messages',
  standalone: true,
  imports: [TimeagoModule, FormsModule],
  templateUrl: './member-messages.component.html',
  styleUrl: './member-messages.component.css'
})

export class MemberMessagesComponent implements AfterViewChecked {
  @ViewChild('messageForm') messageForm?: NgForm;
  @ViewChild('scrollMe') scrollContainer?: any;
  messagesService = inject(MessagesService);
  userId = input.required<number>();
  messageContent = '';


  sendMessage() {
    this.messagesService.sendMessage(this.userId(), this.messageContent).then(() => {
      this.messageForm?.reset();
      this.scrollToBottom();
    })
  }

  ngAfterViewChecked(): void {
    this.scrollToBottom();
  }

  private scrollToBottom() {
    if (this.scrollContainer) {
      this.scrollContainer.nativeElement.scrollTop = this.scrollContainer.nativeElement.scrollHeight;
    }
  }
}
