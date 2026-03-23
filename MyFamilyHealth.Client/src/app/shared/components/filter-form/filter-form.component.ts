import { Component, EventEmitter, Output, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { TranslocoModule } from '@jsverse/transloco';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { DatePickerModule } from 'primeng/datepicker';
import { buildFeatureFilterForm } from './filter-form.form';

@Component({
  selector: 'app-filter-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, TranslocoModule, InputTextModule, SelectModule, DatePickerModule],
  templateUrl: './filter-form.component.html'
})
export class FilterFormComponent {
  private fb = inject(FormBuilder);
  
  form: FormGroup = buildFeatureFilterForm(this.fb);
  
  @Output() filterChanged = new EventEmitter<any>();

  types = [
    { label: 'All Types', value: 'all' },
    { label: 'Laboratory', value: 'lab' },
    { label: 'Imaging', value: 'imaging' },
    { label: 'Prescription', value: 'rx' }
  ];

  providers = [
    { label: 'All Providers', value: null },
    { label: 'Dr. Helena Vance', value: 'dr_helena' },
    { label: 'Dr. Marcus Thorne', value: 'dr_marcus' },
    { label: 'Dr. Sarah Chen', value: 'dr_sarah' }
  ];

  constructor() {
    this.form.valueChanges.subscribe(val => {
      this.filterChanged.emit(val);
    });
  }
}
