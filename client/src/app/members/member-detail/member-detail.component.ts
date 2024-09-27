import { Component, inject, OnInit } from '@angular/core';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { MembersService } from '../../_services/members.service';
import { ActivatedRoute } from '@angular/router';
import { Member } from '../../_models/member';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';

@Component({
  selector: 'app-member-detail',
  standalone: true,
  imports: [TabsModule, GalleryModule],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css'
})

export class MemberDetailComponent implements OnInit {

  private memberService = inject(MembersService);
  private routes = inject(ActivatedRoute);

  member?: Member;
  images: GalleryItem[] = [];

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    const id = this.routes.snapshot.paramMap.get('id');
    if (id) {
      this.memberService.getMemberById(id).subscribe(
        {
          next: member => {
            this.member = member;
            member.photos.map(p => {
              this.images.push(new ImageItem({
                src: p.url,
                thumb: p.url
              }))
            })
          }
        }
      );
    }
  }
}
