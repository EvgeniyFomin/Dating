import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';
import { of, tap } from 'rxjs';
import { Photo } from '../_models/photo';
import { PaginatedResult } from '../_models/pagination';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})

export class MembersService {
  private httpClient = inject(HttpClient);
  baseUrl = environment.apiUrl + "users/";
  // members = signal<Member[]>([]);
  paginatedResult = signal<PaginatedResult<Member[]> | null>(null);

  getMembers(pageNumber?: number, pageSize?: number) {
    let params = new HttpParams();
    if (pageNumber && pageSize) {
      params = params.append('pageNumber', pageNumber);
      params = params.append('pageSize', pageSize);
    }

    return this.httpClient.get<Member[]>(this.baseUrl, { observe: 'response', params: params }).subscribe({
      next: response => {
        this.paginatedResult.set({
          items: response.body as Member[],
          pagination: JSON.parse(response.headers.get('Pagination')!)
        })
      }
    });
  }

  getMemberByName(userName: string) {
    // const member = this.members().find(x => x.userName.toLowerCase() === userName.toLowerCase());

    // if (member !== undefined) return of(member);

    return this.httpClient.get<Member>(this.baseUrl + userName);
  }

  getMemberById(id: string) {
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
}
