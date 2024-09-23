import { AccountService } from './account.service';
import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';

@Injectable({
  providedIn: 'root'
})

export class MembersService {
  private hpptClient = inject(HttpClient);
  private accountService = inject(AccountService);

  baseUrl = environment.apiUrl + "users/";
  members: any = [];

  getMembers() {
    return this.hpptClient.get<Member[]>(this.baseUrl);
  }

  getMemberByName(userName: string) {
    return this.hpptClient.get<Member>(this.baseUrl + userName);
  }
}
