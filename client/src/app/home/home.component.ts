import { Component, inject, OnInit } from '@angular/core';
import { RegisterComponent } from "../register/register.component";
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  httpClient = inject(HttpClient);
  isRegisterMode = false;
  users: any;

  ngOnInit(): void {
    this.getUsers();
  }

  registerToggle() {
    this.isRegisterMode = !this.isRegisterMode;
  }
  getUsers() {
    this.httpClient.get('https://localhost:5001/users').subscribe({
      next: users => this.users = users,
      error: error => console.log(error),
      complete: () => console.log('users got successfully')
    })
  }
}
