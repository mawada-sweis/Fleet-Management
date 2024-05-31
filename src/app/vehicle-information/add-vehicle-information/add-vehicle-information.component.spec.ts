import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddVehicleInformationComponent } from './add-vehicle-information.component';

describe('AddVehicleInformationComponent', () => {
  let component: AddVehicleInformationComponent;
  let fixture: ComponentFixture<AddVehicleInformationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddVehicleInformationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddVehicleInformationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
