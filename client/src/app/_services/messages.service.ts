import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Message } from '../_models/message';
import { PaginatedResult } from '../_models/pagination';
import { MessageParams } from '../_models/messageParams';
import { setPaginatedResponse, setPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MessagesService {
  private httpClient = inject(HttpClient);
  baseUrl = environment.apiUrl + 'messages/';
  paginatedResult = signal<PaginatedResult<Message[]> | null>(null);

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

  sendMessage(recepientId: number, content: string) {
    return this.httpClient.post<Message>(this.baseUrl, { recipientId: recepientId, content: content });
  }

  deleteMessage(messageId: number) {
    return this.httpClient.delete(this.baseUrl + messageId);
  }
}
