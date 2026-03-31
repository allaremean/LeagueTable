import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.currentUserValue) {
    const requiredRoles = route.data['roles'] as Array<string>;
    if (requiredRoles) {
      if (requiredRoles.includes(authService.currentUserValue.role)) {
        return true;
      } else {
        router.navigate(['/']);
        return false;
      }
    }
    return true;
  }

  router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
  return false;
};
