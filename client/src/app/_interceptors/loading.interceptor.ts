import { inject } from '@angular/core';
import { LoaderService } from './../_services/loader.service';
import { HttpInterceptorFn } from '@angular/common/http';
import { finalize } from 'rxjs';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const loaderService = inject(LoaderService);
  loaderService.show();

  return next(req).pipe(
    finalize(() => {
      loaderService.hide()
    })
  );
};
