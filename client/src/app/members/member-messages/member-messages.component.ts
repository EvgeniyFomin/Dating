import { TimeagoModule } from 'ngx-timeago';
import { Component, inject, input, OnInit } from '@angular/core';
import { MessagesService } from '../../_services/messages.service';
import { Message } from '../../_models/message';

@Component({
  selector: 'app-member-messages',
  standalone: true,
  imports: [TimeagoModule],
  templateUrl: './member-messages.component.html',
  styleUrl: './member-messages.component.css'
})

export class MemberMessagesComponent implements OnInit {
  messagesService = inject(MessagesService);
  userId = input.required<number>();
  messages = input.required<Message[]>();

  ngOnInit(): void {

  }

}
