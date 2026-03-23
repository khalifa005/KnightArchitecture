import { TranslocoModule } from '@jsverse/transloco';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { Component } from '@angular/core';

@Component({
  standalone: true,
  selector: 'app-access-control',
  imports: [TranslocoModule, ButtonModule, InputTextModule],
  templateUrl: './access-control.html',
  styleUrl: './access-control.scss',
})
export class AccessControl {}
