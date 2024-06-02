import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateVehicleInformationComponent } from './update-vehicle-information.component';

describe('UpdateVehicleInformationComponent', () => {
  let component: UpdateVehicleInformationComponent;
  let fixture: ComponentFixture<UpdateVehicleInformationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UpdateVehicleInformationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UpdateVehicleInformationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
