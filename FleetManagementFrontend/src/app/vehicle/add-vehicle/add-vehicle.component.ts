import { Component, Output, EventEmitter } from '@angular/core';
import { VehicleService } from 'src/app/services/vehicle.service';
import { AddVehicleResponse, Vehicle } from './add-vehicle.model';

@Component({
  selector: 'app-add-vehicle',
  templateUrl: './add-vehicle.component.html',
  styleUrls: ['./add-vehicle.component.css'],
})
export class AddVehicleComponent {
  @Output() vehicleAdded = new EventEmitter<Vehicle>();
  newVehicle: Vehicle = {
    VehicleNumber: '',
    VehicleType: '',
    VehicleID: '',
  };

  constructor(private vehicleService: VehicleService) {}

  addVehicle(): void {
    this.vehicleService.addVehicle(this.newVehicle).subscribe(
      (response: AddVehicleResponse) => {
        console.log('Vehicle added successfully', response);
        if (
          response.DicOfDT &&
          response.DicOfDT.Vehicles &&
          response.DicOfDT.Vehicles.length > 0
        ) {
          this.vehicleAdded.emit(response.DicOfDT.Vehicles[0]);
          this.newVehicle = {
            VehicleID: '',
            VehicleNumber: '',
            VehicleType: '',
          };
        }
      },
      (error) => {
        console.error('Error adding vehicle', error);
      }
    );
  }
}
