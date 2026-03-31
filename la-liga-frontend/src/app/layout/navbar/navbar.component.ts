import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { Observable } from 'rxjs';
import { User } from '../../models/interfaces';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent {
  authService = inject(AuthService);
  router = inject(Router);
  
  currentUser$: Observable<User | null> = this.authService.currentUser$;

  logout() {
    this.authService.logout();
    this.router.navigate(['/']);
  }

  isAdmin(user: User): boolean {
    return user.role === 'Admin' || user.role === 'SuperAdmin';
  }
}
