import { FormsModule } from '@angular/forms';
import { Member } from '../_models/member';
import { LikesService } from './../_services/likes.service';
import { Component, inject, OnInit } from '@angular/core';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { MemberCardComponent } from "../members/member-card/member-card.component";

@Component({
  selector: 'app-lists',
  standalone: true,
  imports: [FormsModule, ButtonsModule, MemberCardComponent],
  templateUrl: './lists.component.html',
  styleUrl: './lists.component.css'
})
export class ListsComponent implements OnInit {
  private likesService = inject(LikesService);
  members: Member[] = [];
  predicate = 'liked';

  ngOnInit(): void {
    this.loadLikes();
  }

  loadLikes() {
    this.likesService.getLikes(this.predicate).subscribe({
      next: response => this.members = response
    });
  }

  getTitle() {
    switch (this.predicate) {
      case 'liked': return 'Members you like';
      case 'likedBy': return 'Members who like you';
      default: return 'Mutual';
    }
  }
}
