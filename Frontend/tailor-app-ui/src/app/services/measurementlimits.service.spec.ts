import { TestBed } from '@angular/core/testing';

import { MeasurementlimitsService } from './measurementlimits.service';

describe('MeasurementlimitsService', () => {
  let service: MeasurementlimitsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MeasurementlimitsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
