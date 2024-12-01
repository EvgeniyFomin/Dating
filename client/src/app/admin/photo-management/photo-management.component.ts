import { map } from 'rxjs';
import { Component, inject, OnInit, signal } from '@angular/core';
import { AdminService } from '../../_services/admin.service';
import { Photo } from '../../_models/photo';

@Component({
  selector: 'app-photo-management',
  standalone: true,
  imports: [],
  templateUrl: './photo-management.component.html',
  styleUrl: './photo-management.component.css'
})

export class PhotoManagementComponent implements OnInit {
  private adminService = inject(AdminService);
  photos = signal<Photo[] | null>(null);

  ngOnInit(): void {
    this.getPhotosForApproval();
  }

  getPhotosForApproval() {
    this.adminService.getPhotosForApproval().subscribe({
      next: photos => this.photos.set(photos),
      error: error => console.error(error)
    })
  }

  approve(photoId: number) {
    this.adminService.approvePhoto(photoId).subscribe({
      next: _ => this.removeFromList(photoId),
      error: error => console.error(error)
    })
  }

  reject(photoId: number) {
    this.adminService.rejectPhoto(photoId).subscribe({
      next: _ => this.removeFromList(photoId),
      error: error => console.error(error)
    })
  }

  private removeFromList(photoId: number) {
    this.photos.update(prev => {
      if (prev) {
        prev.splice(prev.findIndex(m => m.id === photoId), 1);
      }
      return prev;
    })
  }
}