import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';

@Injectable({
  providedIn: 'root'
})

export class LikesService {
  private baseUrl = environment.apiUrl + 'likes';
  private httpClient = inject(HttpClient);
  likeIds = signal<number[]>([]);

  toggleLike(targetUserId: number) {
    return this.httpClient.post(this.baseUrl + '/' + targetUserId, {});
  }

  getLikeIds() {
    return this.httpClient.get<number[]>(this.baseUrl + '/list').subscribe({
      next: response => this.likeIds.set(response)
    });
  }

  getLikes(predicate: string) {
    return this.httpClient.get<Member[]>(this.baseUrl + '?predicate=' + predicate);
  }
}
