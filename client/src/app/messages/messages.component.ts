import { Component, inject, OnInit } from '@angular/core';
import { MessagesService } from '../_services/messages.service';
import { Message } from '../_models/message';
import { MessageParams } from '../_models/messageParams';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { FormsModule } from '@angular/forms';
import { TimeagoModule } from 'ngx-timeago';
import { RouterLink } from '@angular/router';
import { PaginationModule } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-messages',
  standalone: true,
  imports: [ButtonsModule, FormsModule, TimeagoModule, RouterLink, PaginationModule],
  templateUrl: './messages.component.html',
  styleUrl: './messages.component.css'
})

export class MessagesComponent implements OnInit {
  messagesService = inject(MessagesService);
  thread: Message[] = [];
  parameters: MessageParams = new MessageParams('1');

  ngOnInit(): void {
    this.loadMessages();

    this.messagesService.getThread(4).subscribe({
      next: messages => this.thread = messages
    });
  }

  loadMessages() {
    this.messagesService.getMessages(this.parameters);
  }

  pageChanged(event: any) {
    if (this.parameters.pageNumber !== event.page) {
      this.parameters.pageNumber = event.page;
      this.loadMessages();
    }
  }

  getRoute(message: Message) {
    if (this.parameters.container === '2') return `/members/${message.recipient.id}`
    else return `/members/${message.sender.id}`
  }
}
