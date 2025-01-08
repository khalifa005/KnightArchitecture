import { Component, Input, TemplateRef } from '@angular/core';

@Component({
  selector: 'app-custom-card',
  templateUrl: './custom-card.component.html',
  styleUrls: ['./custom-card.component.scss']
})
export class CustomCardComponent {
  @Input() imageUrl: string = ''; // Image URL for the card
  @Input() section1Template?: TemplateRef<any>; // Template for the first section
  @Input() section2Template?: TemplateRef<any>; // Template for the second section
}

