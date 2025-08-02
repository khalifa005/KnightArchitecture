import { Component } from '@angular/core';
import { SHARED_IMPORTS } from '@shared';

@Component({
  selector: 'app-guard-leave',
  template: `
    <p>Confirmation required when leaving</p>
    <button nz-button [nzType]="'primary'" [routerLink]="['/delon/guard']">
      <span>I want to leave</span>
    </button>
  `,
  imports: SHARED_IMPORTS
})
export class GuardLeaveComponent {}
