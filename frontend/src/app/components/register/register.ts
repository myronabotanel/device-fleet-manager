import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  imports: [CommonModule, FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {
  name = '';
  email = '';
  password = '';
  role = '';
  location = '';
  errorMessage = '';

  constructor(private authService: AuthService, private router: Router) {}

  register(): void {
    if (!this.name || !this.email || !this.password || !this.role || !this.location) {
      this.errorMessage = 'All fields are required!';
      return;
    }

    this.authService.register({
      name: this.name,
      email: this.email,
      password: this.password,
      role: this.role,
      location: this.location
    }).subscribe({
      next: () => {
        this.router.navigate(['/login']);
      },
      error: () => {
        this.errorMessage = 'Email already exists!';
      }
    });
  }

  goToLogin(): void {
    this.router.navigate(['/login']);
  }
}