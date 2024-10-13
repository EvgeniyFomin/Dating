import { Component, inject, NgModule, OnInit } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { MemberCardComponent } from "../member-card/member-card.component";
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { UserParams } from '../../_models/userParams';
import { AccountService } from '../../_services/account.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [MemberCardComponent, PaginationModule, FormsModule],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css'
})

export class MemberListComponent implements OnInit {
  private accountService = inject(AccountService);
  membersService = inject(MembersService);
  userParameters = new UserParams(this.accountService.currentUser());
  genderList = [
    { value: '1', display: 'Males' },
    { value: '2', display: 'Females' },
    { value: '3', display: 'Others' }
  ]

  ngOnInit(): void {
    if (!this.membersService.paginatedResult()) this.loadMembers();
  }

  loadMembers() {
    this.membersService.getMembers(this.userParameters);
  }

  pageChanged(event: any) {
    if (this.userParameters.pageNumber !== event.page) {
      this.userParameters.pageNumber = event.page;
      this.loadMembers();
    }
  }

  resetFilters() {
    this.userParameters = new UserParams(this.accountService.currentUser())
  }
}
