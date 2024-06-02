import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddRoutHistoryComponent } from './add-rout-history.component';

describe('AddRoutHistoryComponent', () => {
  let component: AddRoutHistoryComponent;
  let fixture: ComponentFixture<AddRoutHistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddRoutHistoryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddRoutHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
