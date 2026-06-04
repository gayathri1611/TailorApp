import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MeasurementsForm } from './measurements-form';

describe('MeasurementsForm', () => {
  let component: MeasurementsForm;
  let fixture: ComponentFixture<MeasurementsForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MeasurementsForm],
    }).compileComponents();

    fixture = TestBed.createComponent(MeasurementsForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
