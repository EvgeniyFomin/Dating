import { UserParams } from './../_models/userParams';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';
import { Photo } from '../_models/photo';
import { PaginatedResult } from '../_models/pagination';
import { of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class MembersService {
  private httpClient = inject(HttpClient);
  baseUrl = environment.apiUrl + "users/";
  paginatedResult = signal<PaginatedResult<Member[]> | null>(null);
  memberCache = new Map();

  getMembers(userParams: UserParams) {
    const response = this.memberCache.get(Object.values(userParams).join('-'));

    if (response) return this.setPaginatedResponse(response);

    let params = this.setPaginationHeaders(userParams);

    return this.httpClient.get<Member[]>(this.baseUrl, { observe: 'response', params: params }).subscribe({
      next: response => {
        this.setPaginatedResponse(response);
        this.memberCache.set(Object.values(userParams).join('-'), response);
      }
    });
  }

  getMemberByName(userName: string) {
    // const member = this.members().find(x => x.userName.toLowerCase() === userName.toLowerCase());

    // if (member !== undefined) return of(member);

    return this.httpClient.get<Member>(this.baseUrl + userName);
  }

  getMemberById(id: string) {
    const member: Member = [...this.memberCache.values()]
      .reduce((arr, elem) => arr.concat(elem.body), [])
      .find((m: Member) => m.id.toString() == id);

    if (member) return of(member);
    // const member = this.members().find(x => x.id.toString() === id);

    // if (member !== undefined) return of(member);

    return this.httpClient.get<Member>(this.baseUrl + id);
  }

  updateMember(member: Member) {
    return this.httpClient.put(this.baseUrl, member).pipe(
      // tap(() => {
      //   this.members.update(members => members.map(m => m.userName === member.userName ? member : m))
      // })
    );
  }

  setMainPhoto(photo: Photo) {
    return this.httpClient.put(this.baseUrl + 'set-main-photo/' + photo.id, {}).pipe(
      // tap(() => {
      //   this.members.update(members => members.map(m => {
      //     if (m.photos.includes(photo)) {
      //       m.mainPhotoUrl = photo.url
      //     }
      //     return m;
      //   }))
      // })
    );
  }

  deletePhoto(photo: Photo) {
    return this.httpClient.delete(this.baseUrl + "delete-photo/" + photo.id).pipe(
      // tap(() => {
      //   this.members.update(members => members.map(m => {
      //     if (m.photos.includes(photo)) {
      //       m.photos = m.photos.filter(x => x.id !== photo.id)
      //     }
      //     return m;
      //   }));
      // })
    );
  }

  private setPaginationHeaders(userParams: UserParams) {
    let params = new HttpParams();

    if (userParams.pageNumber && userParams.pageSize) {
      params = params.append('pageNumber', userParams.pageNumber);
      params = params.append('pageSize', userParams.pageSize);
    }

    params = params.append('minAge', userParams.minAge);
    params = params.append('maxAge', userParams.maxAge);
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);

    return params;
  }

  private setPaginatedResponse(response: HttpResponse<Member[]>) {
    this.paginatedResult.set({
      items: response.body as Member[],
      pagination: JSON.parse(response.headers.get('Pagination')!)
    })
  }
}