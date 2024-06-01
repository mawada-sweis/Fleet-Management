import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetRoutHistoryComponent } from './get-rout-history.component';

describe('GetRoutHistoryComponent', () => {
  let component: GetRoutHistoryComponent;
  let fixture: ComponentFixture<GetRoutHistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GetRoutHistoryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GetRoutHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
