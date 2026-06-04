import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FabricForm } from './fabric-form';

describe('FabricForm', () => {
  let component: FabricForm;
  let fixture: ComponentFixture<FabricForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FabricForm],
    }).compileComponents();

    fixture = TestBed.createComponent(FabricForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
