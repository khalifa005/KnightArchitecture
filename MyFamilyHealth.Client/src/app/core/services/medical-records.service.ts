import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MedicalRecord } from '../models/medical-record.model';

@Injectable({
  providedIn: 'root'
})
export class MedicalRecordsService {
  private http = inject(HttpClient);

  getRecords(): Observable<MedicalRecord[]> {
    return this.http.get<MedicalRecord[]>('/assets/data/medical-records.json');
  }
}
