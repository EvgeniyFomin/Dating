import { PaginatedResult } from './../_models/pagination';
import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';
import { setPaginatedResponse, setPaginationHeaders } from './paginationHelper';
import { LikeParams } from '../_models/likeParams';

@Injectable({
  providedIn: 'root'
})

export class LikesService {
  private baseUrl = environment.apiUrl + 'likes/';
  private httpClient = inject(HttpClient);
  likeIds = signal<number[]>([]);
  paginatedResult = signal<PaginatedResult<Member[]> | null>(null);

  toggleLike(targetUserId: number) {
    return this.httpClient.post(this.baseUrl + targetUserId, {});
  }

  getLikeIds() {
    return this.httpClient.get<number[]>(this.baseUrl + 'list').subscribe({
      next: response => this.likeIds.set(response)
    });
  }

  getLikes(likeParams: LikeParams) {
    let params = setPaginationHeaders(likeParams);
    params = params.append('predicate', likeParams.predicate);

    return this.httpClient.get<Member[]>(this.baseUrl, { observe: 'response', params }).subscribe({
      next: response => setPaginatedResponse(response, this.paginatedResult)
    });
  }
}
