import { Component, inject } from '@angular/core';
import { MembersService } from '../../_services/members.service';

@Component({
  selector: 'app-member-detail',
  standalone: true,
  imports: [],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css'
})
export class MemberDetailComponent {
private memberService = inject(MembersService);

}
