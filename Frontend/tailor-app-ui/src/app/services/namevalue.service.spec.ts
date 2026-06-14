import { TestBed } from '@angular/core/testing';

import { NamevalueService } from './namevalue.service';

describe('NamevalueService', () => {
  let service: NamevalueService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NamevalueService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
