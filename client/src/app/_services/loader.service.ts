import { inject, Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})

export class LoaderService {
  private spinnerService = inject(NgxSpinnerService);
  busyRequestCount = 0;

  show() {
    this.busyRequestCount++;
    this.spinnerService.show(undefined, {
      size: "medium",
      bdColor: "rgba(255, 255, 255, 0)",
      color: "#db1616"
    })
  }

  hide() {
    this.busyRequestCount--;
    if (this.busyRequestCount <= 0) {
      this.busyRequestCount = 0;
      this.spinnerService.hide();
    }
  }
}
