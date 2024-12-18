import { Photo } from './../../_models/photo';
import { Component, inject, input, OnInit, output } from '@angular/core';
import { Member } from '../../_models/member';
import { DecimalPipe, NgClass, NgFor, NgIf, NgStyle } from '@angular/common';
import { FileUploader, FileUploadModule } from 'ng2-file-upload';
import { AccountService } from '../../_services/account.service';
import { environment } from '../../../environments/environment';
import { ToastrService } from 'ngx-toastr';
import { MembersService } from '../../_services/members.service';

@Component({
  selector: 'app-photo-editor',
  standalone: true,
  imports: [NgIf, NgFor, NgStyle, NgClass, FileUploadModule, DecimalPipe],
  templateUrl: './photo-editor.component.html',
  styleUrl: './photo-editor.component.css'
})

export class PhotoEditorComponent implements OnInit {
  private accountService = inject(AccountService);
  private memberService = inject(MembersService);
  private toastr = inject(ToastrService);
  member = input.required<Member>();
  memberChange = output<Member>();
  uploader?: FileUploader;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;

  ngOnInit(): void {
    this.initializeUploader();
  }
  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/add-photo',
      authToken: 'Bearer ' + this.accountService.currentUser()?.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false
    }

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      const photo = JSON.parse(response);
      const updatedMember = { ...this.member() }
      updatedMember.photos.push(photo);

      if (photo.isMain) {
        this.updateMainPhoto(photo, updatedMember);
      }

      this.memberChange.emit(updatedMember);
    }

    this.uploader.onErrorItem = (item, response, status, headers) => {
      const resp = JSON.parse(response);
      this.toastr.error(resp?.message);
    }
  }

  setMainPhoto(photo: Photo) {
    this.memberService.setMainPhoto(photo).subscribe({
      next: _ => {
        const updatedMember = { ...this.member() }
        this.updateMainPhoto(photo, updatedMember);
        this.memberChange.emit(updatedMember);
      }
    })
  }

  deletePhoto(photo: Photo) {
    this.memberService.deletePhoto(photo).subscribe({
      next: _ => {
        const updatedMember = { ...this.member() }
        updatedMember.photos = updatedMember.photos.filter(x => x.id !== photo.id);
        this.memberChange.emit(updatedMember);
      }
    })
  }

  private updateMainPhoto(photo: Photo, member: Member) {
    const user = this.accountService.currentUser();
    if (user) {
      user.photoUrl = photo.url;
      this.accountService.setCurrentUser(user);
    }
    member.mainPhotoUrl = photo.url;
    member.photos.forEach(p => {
      if (p.isMain) p.isMain = false;
      if (p.id === photo.id) p.isMain = true;
    });
  }
}
