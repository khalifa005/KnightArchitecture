import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TabScrollService {
  private canScrollLeft = new BehaviorSubject<boolean>(false);
  private canScrollRight = new BehaviorSubject<boolean>(false);
  private fitToWidth = new BehaviorSubject<boolean>(false);

  canScrollLeft$ = this.canScrollLeft.asObservable();
  canScrollRight$ = this.canScrollRight.asObservable();
  fitToWidth$ = this.fitToWidth.asObservable();

  updateScrollButtons(element: HTMLElement): void {
    const hasHorizontalScroll = element.scrollWidth > element.clientWidth;
    this.canScrollLeft.next(element.scrollLeft > 0);
    this.canScrollRight.next(
      hasHorizontalScroll && element.scrollLeft < element.scrollWidth - element.clientWidth
    );
  }

  scrollLeft(element: HTMLElement): void {
    element.scrollBy({ left: -200, behavior: 'smooth' });
  }

  scrollRight(element: HTMLElement): void {
    element.scrollBy({ left: 200, behavior: 'smooth' });
  }

  toggleFitToWidth(value: boolean): void {
    this.fitToWidth.next(value);
  }
}