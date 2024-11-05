import { LikesService } from './../../_services/likes.service';
import { Component, computed, inject, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { TabDirective, TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs';
import { ActivatedRoute } from '@angular/router';
import { Member } from '../../_models/member';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TimeagoModule } from 'ngx-timeago';
import { DatePipe } from '@angular/common';
import { MemberMessagesComponent } from "../member-messages/member-messages.component";
import { Message } from '../../_models/message';
import { MessagesService } from '../../_services/messages.service';
import { PresenceService } from '../../_services/presence.service';
import { AccountService } from '../../_services/account.service';

@Component({
  selector: 'app-member-detail',
  standalone: true,
  imports: [TabsModule, GalleryModule, TimeagoModule, DatePipe, MemberMessagesComponent],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css'
})

export class MemberDetailComponent implements OnInit, OnDestroy {

  @ViewChild('memberTabs', { static: true }) memberTabs?: TabsetComponent;
  private route = inject(ActivatedRoute);
  private messagesService = inject(MessagesService);
  private likeService = inject(LikesService);
  private accountService = inject(AccountService);
  presenceService = inject(PresenceService);

  hasLiked = computed(() => this.likeService.likeIds().includes(this.member.id));
  member: Member = {} as Member;
  images: GalleryItem[] = [];
  activeTab?: TabDirective;

  ngOnInit(): void {
    this.route.data.subscribe({
      next: data => {
        this.member = data['member'];
        this.member && this.member.photos.map(p => {
          this.images.push(new ImageItem({
            src: p.url,
            thumb: p.url
          }))
        })
      }
    })

    this.route.queryParams.subscribe({
      next: params => {
        params['tab'] && this.selectTab(params['tab'])
      }
    })
  }

  ngOnDestroy(): void {
    this.messagesService.stopHubConnection();
  }

  onTabActivated(data: TabDirective) {
    this.activeTab = data;
    if (this.activeTab.heading === 'Messages' && this.member) {
      const user = this.accountService.currentUser();
      if (!user) return;

      this.messagesService.createHubConnection(user, this.member.id);
    } else {
      this.messagesService.stopHubConnection();
    }
  }

  selectTab(tabHeading: string) {
    if (this.memberTabs) {
      const tab = this.memberTabs.tabs.find(x => x.heading?.toLocaleLowerCase() === tabHeading.toLocaleLowerCase());
      if (tab) {
        tab.active = true;
      }
    }
  }

  toggleLike() {
    this.likeService.toggleLike(this.member.id).subscribe({
      next: () => {
        if (this.hasLiked()) {
          this.likeService.likeIds.update(ids => ids.filter(x => x !== this.member.id))
        }
        else {
          this.likeService.likeIds.update(ids => [...ids, this.member.id])
        }
      }
    }
    );
  }
}
