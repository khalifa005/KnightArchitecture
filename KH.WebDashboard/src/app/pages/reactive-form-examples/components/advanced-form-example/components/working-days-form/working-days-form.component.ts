import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-working-days-form',
  templateUrl: './working-days-form.component.html',
  styleUrl: './working-days-form.component.scss'
})
export class WorkingDaysFormComponent {
  @Input() workingDayGroup!: FormGroup;
  @Output() remove = new EventEmitter<void>();

  onRemoveWorkingDay(): void {
    this.remove.emit();
  }
}