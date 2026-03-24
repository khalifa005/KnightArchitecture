import { TranslocoModule, TranslocoService } from '@jsverse/transloco';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DialogModule } from 'primeng/dialog';
import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { toSignal } from '@angular/core/rxjs-interop';
import { BehaviorSubject, combineLatest, map } from 'rxjs';
import { FilterFormComponent } from '../../shared/components/filter-form/filter-form.component';
import { MedicalRecordsService } from '../../core/services/medical-records.service';

@Component({
  standalone: true,
  selector: 'app-medical-records',
  imports: [CommonModule, TranslocoModule, ButtonModule, InputTextModule, FilterFormComponent, DialogModule],
  templateUrl: './medical-records.html',
  styleUrl: './medical-records.scss',
})
export class MedicalRecords {
  private recordsService = inject(MedicalRecordsService);
  private transloco = inject(TranslocoService);
  
  // Real-time Filters Subscriptions
  private filters$ = new BehaviorSubject<any>({ type: 'all', provider: null, searchQuery: '' });
  
  // Base Data Subject for "Load More" functionality
  private allRecordsRaw$ = new BehaviorSubject<any[]>([]);

  // Derived Filtered Signal
  records = toSignal(
    combineLatest([this.allRecordsRaw$, this.filters$]).pipe(
      map(([records, filters]) => {
        let result = [...records];
        
        // Match Search Query against translated strings
        if (filters.searchQuery) {
          const query = filters.searchQuery.toLowerCase();
          result = result.filter(r => this.transloco.translate(r.titleKey).toLowerCase().includes(query));
        }

        // Match Dropdown Type
        if (filters.type && filters.type !== 'all') {
          result = result.filter(r => r.type === filters.type);
        }

        // Match Provider
        if (filters.provider) {
          result = result.filter(r => r.providerKey === filters.provider || r.providerKey.includes(filters.provider));
        }
        
        return result;
      })
    ),
    { initialValue: [] }
  );

  // Modal State Signal
  displayAddModal = signal(false);

  constructor() {
    // Initial Hydration
    this.recordsService.getRecords().subscribe(data => {
      this.allRecordsRaw$.next(data);
    });
  }

  onFilterChanged(filters: any) {
    this.filters$.next(filters);
  }

  loadMore() {
    // Simulates an API pagination dump by cloning records
    const current = this.allRecordsRaw$.getValue();
    const mockRecord = { ...current[0], id: `REC-${Math.floor(Math.random() * 1000)}`, isUrgent: false };
    this.allRecordsRaw$.next([...current, mockRecord]);
  }

  openAddModal() {
    this.displayAddModal.set(true);
  }
}
