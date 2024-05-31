import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetVehicleInformationComponent } from './get-vehicle-information.component';

describe('GetVehicleInformationComponent', () => {
  let component: GetVehicleInformationComponent;
  let fixture: ComponentFixture<GetVehicleInformationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GetVehicleInformationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GetVehicleInformationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
