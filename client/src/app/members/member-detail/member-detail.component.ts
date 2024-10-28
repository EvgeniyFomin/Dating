import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { TabDirective, TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs';
import { MembersService } from '../../_services/members.service';
import { ActivatedRoute } from '@angular/router';
import { Member } from '../../_models/member';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TimeagoModule } from 'ngx-timeago';
import { DatePipe } from '@angular/common';
import { MemberMessagesComponent } from "../member-messages/member-messages.component";
import { Message } from '../../_models/message';
import { MessagesService } from '../../_services/messages.service';

@Component({
  selector: 'app-member-detail',
  standalone: true,
  imports: [TabsModule, GalleryModule, TimeagoModule, DatePipe, MemberMessagesComponent],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css'
})

export class MemberDetailComponent implements OnInit {
  @ViewChild('memberTabs') memberTabs?: TabsetComponent;
  private memberService = inject(MembersService);
  private routes = inject(ActivatedRoute);
  private messagesService = inject(MessagesService);
  member?: Member;
  images: GalleryItem[] = [];
  activeTab?: TabDirective;
  messages: Message[] = [];

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

  onTabActivated(data: TabDirective) {
    this.activeTab = data;
    if (this.activeTab.heading === 'Messages' && this.messages.length === 0 && this.member) {
      this.messagesService.getThread(this.member.id).subscribe({
        next: messages => this.messages = messages
      })
    }
  }
}
