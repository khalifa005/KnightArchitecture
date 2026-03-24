import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FamilyProfilesComponent } from './family-profiles.component';
import { getTranslocoModule } from '../../core/testing/transloco-testing.module';

describe('FamilyProfilesComponent', () => {
  let component: FamilyProfilesComponent;
  let fixture: ComponentFixture<FamilyProfilesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FamilyProfilesComponent, getTranslocoModule()]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(FamilyProfilesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
