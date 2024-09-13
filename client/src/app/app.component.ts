import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavComponent } from "./nav/nav.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'Dating Client';
  httpClient = inject(HttpClient);
  users: any;

  ngOnInit(): void {
    this.httpClient.get('https://localhost:5001/users').subscribe({
      next: users => this.users = users,
      error: error => console.log(error),
      complete: () => console.log('users got successfully')
    })
  }
}
