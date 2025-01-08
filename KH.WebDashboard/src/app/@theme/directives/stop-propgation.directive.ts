import { Directive, ElementRef, HostListener } from '@angular/core';

@Directive({
  selector: '[ngxStopPropgation]'
})
export class StopPropgationDirective {

  @HostListener('click', ['$event'])
  public onClick(event: any): void {
    event.stopPropagation();
  }
  @HostListener('dblclick', ['$event'])
  public onDbClick(event: any): void {
    event.stopPropagation();
  }

}
