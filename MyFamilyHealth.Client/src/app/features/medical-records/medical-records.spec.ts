import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MedicalRecords } from './medical-records';

describe('MedicalRecords', () => {
  let component: MedicalRecords;
  let fixture: ComponentFixture<MedicalRecords>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MedicalRecords],
    }).compileComponents();

    fixture = TestBed.createComponent(MedicalRecords);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
