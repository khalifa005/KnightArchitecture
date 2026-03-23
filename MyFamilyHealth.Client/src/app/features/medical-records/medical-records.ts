import { TranslocoModule } from '@jsverse/transloco';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { Component } from '@angular/core';
import { FilterFormComponent } from '../../shared/components/filter-form/filter-form.component';

@Component({
  standalone: true,
  selector: 'app-medical-records',
  imports: [TranslocoModule, ButtonModule, InputTextModule, FilterFormComponent],
  templateUrl: './medical-records.html',
  styleUrl: './medical-records.scss',
})
export class MedicalRecords {
  onFilterChanged(filters: any) {
    console.log('Filters updated via Reactive Form:', filters);
  }
}
