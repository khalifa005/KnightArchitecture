import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-dynamic-svg',
  templateUrl: './dynamic-svg.component.html',
  styleUrls: ['./dynamic-svg.component.scss']
})
export class DynamicSvgComponent {
  @Input() width: string = '250';
  @Input() height: string = '250';
  @Input() viewBox: string = '0 0 35 40';
  @Input() fillColor: string = '#F97316';
  @Input() pathData: string = '';

  constructor() {}
}
