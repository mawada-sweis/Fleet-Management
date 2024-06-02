import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteVehicleInformationComponent } from './delete-vehicle-information.component';

describe('DeleteVehicleInformationComponent', () => {
  let component: DeleteVehicleInformationComponent;
  let fixture: ComponentFixture<DeleteVehicleInformationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeleteVehicleInformationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DeleteVehicleInformationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
