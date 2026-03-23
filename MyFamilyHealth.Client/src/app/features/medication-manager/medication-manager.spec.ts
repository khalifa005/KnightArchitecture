import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MedicationManager } from './medication-manager';

describe('MedicationManager', () => {
  let component: MedicationManager;
  let fixture: ComponentFixture<MedicationManager>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MedicationManager],
    }).compileComponents();

    fixture = TestBed.createComponent(MedicationManager);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
