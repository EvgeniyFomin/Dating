import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Message } from '../_models/message';
import { PaginatedResult } from '../_models/pagination';
import { MessageParams } from '../_models/messageParams';
import { setPaginatedResponse, setPaginationHeaders } from './paginationHelper';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { User } from '../_models/user';
import { Group } from '../_models/group';

@Injectable({
  providedIn: 'root'
})
export class MessagesService {
  private httpClient = inject(HttpClient);
  private baseUrl = environment.apiUrl + 'messages/';
  private hubUrl = environment.hubsUrl + 'message';
  hubConnection?: HubConnection;
  paginatedResult = signal<PaginatedResult<Message[]> | null>(null);
  messageThread = signal<Message[]>([]);

  getThread(id: number) {
    return this.httpClient.get<Message[]>(this.baseUrl + 'thread/' + id);
  }

  getMessages(parameters: MessageParams) {
    let params = setPaginationHeaders(parameters);
    params = params.append('container', parameters.container);

    return this.httpClient.get<Message[]>(this.baseUrl, { observe: 'response', params }).subscribe({
      next: response => setPaginatedResponse(response, this.paginatedResult)
    });
  }

  async sendMessage(recipientId: number, content: string) {
    return this.hubConnection?.invoke('SendMessage', { recipientId: recipientId, content: content })
  }

  deleteMessage(messageId: number) {
    return this.httpClient.delete(this.baseUrl + messageId);
  }

  createHubConnection(user: User, otherUserId: number) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + '?userId=' + otherUserId, {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch(error => console.error(error));

    this.hubConnection.on('ReceiveMessageThread', messages => {
      this.messageThread.set(messages);
    })

    this.hubConnection.on('NewMessage', message => {
      this.messageThread.update(messages => [...messages, message])
    })

    this.hubConnection.on("UpdatedGroup", (group: Group) => {
      if (group.connections.some(x => x.userId === otherUserId)) {
        this.messageThread.update(messages => {
          messages.forEach(message => {
            if (!message.readDate) {
              message.readDate = new Date(Date.now());
            }
          });
          return messages;
        })
      }
    })
  }

  stopHubConnection() {
    if (this.hubConnection?.state === HubConnectionState.Connected) {
      this.hubConnection.stop().catch(error => console.error(error));
    }
  }
}
