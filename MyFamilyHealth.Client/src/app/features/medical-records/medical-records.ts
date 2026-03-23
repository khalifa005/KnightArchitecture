import { TranslocoModule } from '@jsverse/transloco';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { toSignal } from '@angular/core/rxjs-interop';
import { FilterFormComponent } from '../../shared/components/filter-form/filter-form.component';
import { MedicalRecordsService } from '../../core/services/medical-records.service';

@Component({
  standalone: true,
  selector: 'app-medical-records',
  imports: [CommonModule, TranslocoModule, ButtonModule, InputTextModule, FilterFormComponent],
  templateUrl: './medical-records.html',
  styleUrl: './medical-records.scss',
})
export class MedicalRecords {
  private recordsService = inject(MedicalRecordsService);
  records = toSignal(this.recordsService.getRecords(), { initialValue: [] });

  onFilterChanged(filters: any) {
    console.log('Filters updated via Reactive Form:', filters);
  }
}
