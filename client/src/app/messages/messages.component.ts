import { Component, inject, OnInit } from '@angular/core';
import { MessagesService } from '../_services/messages.service';
import { Message } from '../_models/message';
import { MessageParams } from '../_models/messageParams';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { FormsModule } from '@angular/forms';
import { TimeagoModule } from 'ngx-timeago';
import { RouterLink } from '@angular/router';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { Container } from '../_enums/container';

@Component({
  selector: 'app-messages',
  standalone: true,
  imports: [ButtonsModule, FormsModule, TimeagoModule, RouterLink, PaginationModule],
  templateUrl: './messages.component.html',
  styleUrl: './messages.component.css'
})

export class MessagesComponent implements OnInit {
  messagesService = inject(MessagesService);
  parameters: MessageParams = new MessageParams(Container.inbox);
  isOutbox = this.parameters.container === Container.outbox;

  ngOnInit(): void {
    this.loadMessages();
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
    if (this.parameters.container === Container.outbox) return `/members/${message.recipient.id}`
    else return `/members/${message.sender.id}`
  }

  container() {
    return Container;
  }

  deleteMessage(messageId: number) {
    this.messagesService.deleteMessage(messageId).subscribe({
      next: _ => this.messagesService.paginatedResult.update(prev => {
        if (prev && prev.items) {
          prev.items.splice(prev.items.findIndex(m => m.id === messageId), 1);
        }
        return prev;
      })
    });
  }
}
