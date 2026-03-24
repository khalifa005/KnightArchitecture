import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { TranslocoModule, TranslocoService } from '@jsverse/transloco';

import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { DatePickerModule } from 'primeng/datepicker';
import { SelectModule } from 'primeng/select';

@Component({
  standalone: true,
  selector: 'app-register',
  imports: [CommonModule, FormsModule, RouterModule, TranslocoModule, InputTextModule, PasswordModule, ButtonModule, CheckboxModule, DatePickerModule, SelectModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent implements OnInit {
  firstName = '';
  lastName = '';
  email = '';
  password = '';
  dob: Date | null = null;
  selectedGender = null;
  acceptTerms = false;
  
  genderOptions: any[] = [
    { label: 'Male', value: 'M' },
    { label: 'Female', value: 'F' },
    { label: 'Prefer not to say', value: 'O' }
  ];
  
  constructor(private router: Router, private transloco: TranslocoService) {}

  ngOnInit() {
      // The gender labels would ideally be mapped to i18n directly using a structural loop
      // or specific transloco pipes in HTML. We will manage this directly in standard arrays here.
  }

  onRegister() {
    // In production, register endpoint would be called here.
    // We immediately bounce the user back to secure login validation layout.
    this.router.navigate(['/login']);
  }
}
