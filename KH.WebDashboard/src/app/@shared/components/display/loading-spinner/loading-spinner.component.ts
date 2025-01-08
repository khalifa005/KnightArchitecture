import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-loading-spinner',
  templateUrl: './loading-spinner.component.html',
  styleUrls: ['./loading-spinner.component.scss']
})
export class LoadingSpinnerComponent {
  @Input() size: string = '5rem'; // Default size if not provided

  sections: number[] = Array(45).fill(0).map((_, index) => index);
  // letters: string[] = ['G', 'N', 'I', 'D', 'A', 'O', 'L'];
}
