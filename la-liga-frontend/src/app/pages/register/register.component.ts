import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  authService = inject(AuthService);
  router = inject(Router);

  username = '';
  password = '';
  error = '';
  isLoading = false;

  onSubmit() {
    this.isLoading = true;
    this.error = '';
    this.authService.register({ username: this.username, password: this.password }).subscribe({
      next: () => {
        this.router.navigate(['/login']);
      },
      error: (err) => {
        this.error = err.error || 'Registration failed. Username may already exist.';
        this.isLoading = false;
      }
    });
  }
}
