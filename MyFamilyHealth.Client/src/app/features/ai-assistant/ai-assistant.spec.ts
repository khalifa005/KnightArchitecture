import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AiAssistant } from './ai-assistant';

describe('AiAssistant', () => {
  let component: AiAssistant;
  let fixture: ComponentFixture<AiAssistant>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AiAssistant],
    }).compileComponents();

    fixture = TestBed.createComponent(AiAssistant);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
