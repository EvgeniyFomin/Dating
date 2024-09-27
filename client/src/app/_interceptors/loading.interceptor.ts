import { inject } from '@angular/core';
import { LoaderService } from './../_services/loader.service';
import { HttpInterceptorFn } from '@angular/common/http';
import { delay, finalize } from 'rxjs';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const loaderService = inject(LoaderService);
  loaderService.busy();

  return next(req).pipe(
    delay(1000),
    finalize(() => {
      loaderService.idle()
    })
  );
};
