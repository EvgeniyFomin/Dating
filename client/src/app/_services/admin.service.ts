import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private baseUrl = environment.apiUrl + "admins/";
  private httpClient = inject(HttpClient);

  getUsersWithRoles() {
    return this.httpClient.get<User[]>(this.baseUrl + 'users-with-roles');
  }

}
