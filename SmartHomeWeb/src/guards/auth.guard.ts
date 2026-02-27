import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authGuard: CanActivateFn = (route, state) => {
  const isNotLoggedIn = localStorage.getItem('token') === null;

  if (isNotLoggedIn) {
    const router = inject(Router);

    return router.parseUrl('login');
  }

  const requiredPermission = route.data['requiredPermission'];
  const permissions = localStorage.getItem('permissions');
  if (requiredPermission && permissions != null && !JSON.parse(permissions).includes(requiredPermission)) {
    const router = inject(Router);

    return router.parseUrl('/home');
  }

  return true;
};
