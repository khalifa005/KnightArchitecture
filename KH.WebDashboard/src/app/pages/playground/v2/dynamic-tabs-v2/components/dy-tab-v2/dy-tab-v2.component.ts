import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-dy-tab-v2',
  templateUrl: './dy-tab-v2.component.html',
  styleUrl: './dy-tab-v2.component.scss'
})
export class DyTabV2Component {
  @Input() content: string = '';

}
