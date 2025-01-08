import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-number-counter',
  templateUrl: './number-counter.component.html',
  styleUrls: ['./number-counter.component.scss']
})
export class NumberCounterComponent implements OnInit {
  @Input() startValue: number = 0;
  @Input() endValue: number = 100;
  @Input() duration: number = 2000; // duration in ms
  @Input() customClass: string; 

  currentNumber: number = this.startValue;

  ngOnInit(): void {
    this.countUp();
  }

  countUp() {
    const interval = 10; // update every 10ms
    const steps = this.duration / interval;
    const increment = (this.endValue - this.startValue) / steps;

    let currentStep = 0;
    const intervalId = setInterval(() => {
      this.currentNumber += increment;
      currentStep++;

      if (currentStep >= steps) {
        this.currentNumber = this.endValue;
        clearInterval(intervalId);
      }
    }, interval);
  }
}