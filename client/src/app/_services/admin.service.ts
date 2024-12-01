import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { User } from '../_models/user';
import { Photo } from '../_models/photo';

@Injectable({
  providedIn: 'root'
})

export class AdminService {
  private baseUrl = environment.apiUrl + "admins/";
  private httpClient = inject(HttpClient);

  getUsersWithRoles() {
    return this.httpClient.get<User[]>(this.baseUrl + 'users-with-roles');
  }

  updateUserRoles(userId: number, roles: string[]) {
    return this.httpClient.post<string[]>(this.baseUrl + 'edit-user-roles/' + userId + '?roles=' + roles, {})
  }

  getPhotosForApproval() {
    return this.httpClient.get<Photo[]>(this.baseUrl + 'photos-for-approval');
  }

  approvePhoto(photoId: number) {
    return this.httpClient.put(this.baseUrl + 'approve-photo/' + photoId, {});
  }

  rejectPhoto(photoId: number) {
    return this.httpClient.delete(this.baseUrl + 'reject-photo/' + photoId);
  }

  removeUser(id: number) {
    return this.httpClient.delete(this.baseUrl + "remove-user/" + id);
  }
}
