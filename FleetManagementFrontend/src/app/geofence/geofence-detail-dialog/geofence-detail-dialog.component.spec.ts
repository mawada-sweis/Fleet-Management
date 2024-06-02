import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GeofenceDetailDialogComponent } from './geofence-detail-dialog.component';

describe('GeofenceDetailDialogComponent', () => {
  let component: GeofenceDetailDialogComponent;
  let fixture: ComponentFixture<GeofenceDetailDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GeofenceDetailDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GeofenceDetailDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
