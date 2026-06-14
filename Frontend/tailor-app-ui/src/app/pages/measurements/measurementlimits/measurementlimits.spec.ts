import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Measurementlimits } from './measurementlimits';

describe('Measurementlimits', () => {
  let component: Measurementlimits;
  let fixture: ComponentFixture<Measurementlimits>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Measurementlimits],
    }).compileComponents();

    fixture = TestBed.createComponent(Measurementlimits);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
