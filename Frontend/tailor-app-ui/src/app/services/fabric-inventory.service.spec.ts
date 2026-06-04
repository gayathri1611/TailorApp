import { TestBed } from '@angular/core/testing';

import { FabricInventoryService } from './fabric-inventory.service';

describe('FabricInventoryService', () => {
  let service: FabricInventoryService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FabricInventoryService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
