import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Namevaluemanager } from './namevaluemanager';

describe('Namevaluemanager', () => {
  let component: Namevaluemanager;
  let fixture: ComponentFixture<Namevaluemanager>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Namevaluemanager],
    }).compileComponents();

    fixture = TestBed.createComponent(Namevaluemanager);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
