import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';

@Component({
  selector: 'app-test-errors',
  standalone: true,
  imports: [],
  templateUrl: './test-errors.component.html',
  styleUrl: './test-errors.component.css'
})
export class TestErrorsComponent {
  baseUrl = 'https://localhost:5001/';
  private httpClient = inject(HttpClient);

  get500error() {
    this.httpClient.get(this.baseUrl + 'buggy/server-error').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    });
  }

  getValidationError() {

    this.httpClient.post(this.baseUrl + 'accounts/register', {}).subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    });
  }

  get400error() {
    this.httpClient.get(this.baseUrl + 'buggy/bad-request').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    });
  }

  get401error() {
    this.httpClient.get(this.baseUrl + 'buggy/unauthorized').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    });
  }

  get404error() {
    this.httpClient.get(this.baseUrl + 'buggy/not-found').subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    });
  }
}
