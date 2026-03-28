
import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../../services/auth-service';
import { catchError, switchMap, throwError } from 'rxjs';

const isAuthEndpoint = (url: string) =>
  url.includes('/api/Auth/Login') ||
  url.includes('/api/Auth/Refresh') ||
  url.includes('/api/Auth/Logout');

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthService);
  const token = auth.token();

  let request = req;

  // Respect bypass header
  const bypassRefresh = request.headers.has('X-Bypass-Refresh');

  // Attach access token only for non-auth endpoints
  if (!isAuthEndpoint(request.url) && token && request.url.includes('/api/')) {
    request = request.clone({
      setHeaders: { Authorization: `Bearer ${token}` }
    });
  }

  // Send credentials only if needed (auth endpoints or cookie flows)
  const needsCreds = isAuthEndpoint(request.url);
  request = request.clone({ withCredentials: needsCreds });

  return next(request).pipe(
    catchError((err: HttpErrorResponse) => {
      const is401 = err.status === 401;
      const alreadyAttempted = request.headers.has('X-Refresh-Attempt');

      // Conditions to trigger refresh:
      const canRefresh =
        is401 &&
        !alreadyAttempted &&
        !bypassRefresh &&
        !isAuthEndpoint(request.url); // don't refresh on auth endpoints

      if (canRefresh) {
        return auth.refresh().pipe(
          switchMap((res: { accessToken: string }) => {
            auth.setToken(res.accessToken);

            const retried = request.clone({
              setHeaders: {
                Authorization: `Bearer ${res.accessToken}`,
                'X-Refresh-Attempt': '1'
              }
              // keep withCredentials from original clone
            });

            return next(retried);
          }),
          catchError(refreshErr => {
            auth.clearToken();
            // Optional: navigate to login or publish a "session expired" event.
            return throwError(() => refreshErr);
          })
        );
      }

      return throwError(() => err);
    })
  );
};