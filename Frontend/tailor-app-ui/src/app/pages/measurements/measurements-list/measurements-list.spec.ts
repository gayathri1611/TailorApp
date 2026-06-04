import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MeasurementsList } from './measurements-list';

describe('MeasurementsList', () => {
  let component: MeasurementsList;
  let fixture: ComponentFixture<MeasurementsList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MeasurementsList],
    }).compileComponents();

    fixture = TestBed.createComponent(MeasurementsList);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
