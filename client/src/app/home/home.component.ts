import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  isRegisterMode = false;

  registerToggle() {
    this.isRegisterMode = !this.isRegisterMode;
  }
}
