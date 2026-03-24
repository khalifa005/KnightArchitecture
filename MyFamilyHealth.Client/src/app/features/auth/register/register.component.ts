import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { TranslocoModule } from '@jsverse/transloco';

import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { ButtonModule } from 'primeng/button';

@Component({
  standalone: true,
  selector: 'app-register',
  imports: [CommonModule, FormsModule, RouterModule, TranslocoModule, InputTextModule, PasswordModule, ButtonModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  fullName = '';
  email = '';
  phone = '';
  nationalId = '';
  password = '';
  confirmPassword = '';
  
  constructor(private router: Router) {}

  onRegister() {
    this.router.navigate(['/login']);
  }
}
