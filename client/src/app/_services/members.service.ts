import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';
import { of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class MembersService {
  private httpClient = inject(HttpClient);
  baseUrl = environment.apiUrl + "users/";
  members = signal<Member[]>([]);

  getMembers() {
    return this.httpClient.get<Member[]>(this.baseUrl).subscribe({
      next: members => this.members.set(members)
    });
  }

  getMemberByName(userName: string) {
    const member = this.members().find(x => x.userName.toLowerCase() === userName.toLowerCase());

    if (member !== undefined) return of(member);

    return this.httpClient.get<Member>(this.baseUrl + userName);
  }

  getMemberById(id: string) {
    const member = this.members().find(x => x.id.toString() === id);

    if (member !== undefined) return of(member);

    return this.httpClient.get<Member>(this.baseUrl + id);
  }

  updateMember(member: Member) {
    return this.httpClient.put(this.baseUrl, member).pipe(
      tap(() => {
        this.members.update(members => members.map(m => m.userName === member.userName ? member : m))
      })
    );
  }
}
