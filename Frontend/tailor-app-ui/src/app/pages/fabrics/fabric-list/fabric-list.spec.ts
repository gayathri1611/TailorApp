import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FabricList } from './fabric-list';

describe('FabricList', () => {
  let component: FabricList;
  let fixture: ComponentFixture<FabricList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FabricList],
    }).compileComponents();

    fixture = TestBed.createComponent(FabricList);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
