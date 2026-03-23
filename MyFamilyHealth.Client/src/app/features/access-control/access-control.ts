import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { Component } from '@angular/core';

@Component({
  selector: 'app-access-control',
  imports: [ButtonModule, InputTextModule],
  templateUrl: './access-control.html',
  styleUrl: './access-control.scss',
})
export class AccessControl {}
