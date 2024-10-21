import { FormsModule } from '@angular/forms';
import { LikesService } from './../_services/likes.service';
import { Component, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { MemberCardComponent } from "../members/member-card/member-card.component";
import { LikeParams } from '../_models/likeParams';
import { PaginationModule } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-lists',
  standalone: true,
  imports: [FormsModule, ButtonsModule, MemberCardComponent, PaginationModule],
  templateUrl: './lists.component.html',
  styleUrl: './lists.component.css'
})

export class ListsComponent implements OnInit, OnDestroy {
  likesService = inject(LikesService);
  likeParams = new LikeParams('liked');

  ngOnInit(): void {
    if (!this.likesService.paginatedResult()) this.loadLikes();
  }

  ngOnDestroy(): void {
    this.likesService.paginatedResult.set(null);
  }

  loadLikes() {
    this.likesService.getLikes(this.likeParams);
  }

  pageChanged(event: any) {
    if (this.likeParams.pageNumber !== event.page) {
      this.likeParams.pageNumber = event.page;
      this.loadLikes();
    }
  }

  getTitle() {
    switch (this.likeParams.predicate) {
      case 'liked': return 'Members you like';
      case 'likedBy': return 'Members who like you';
      default: return 'Mutual';
    }
  }
}
