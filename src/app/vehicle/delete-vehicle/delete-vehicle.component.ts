import { Component, OnInit } from '@angular/core';
import { VehicleService } from '../../services/vehicle.service';
import { Vehicle, DeleteVehicleResponse } from './delete-vehicle.model';

@Component({
  selector: 'app-delete-vehicle',
  templateUrl: './delete-vehicle.component.html',
  styleUrls: ['./delete-vehicle.component.css']
})
export class DeleteVehicleComponent {
  constructor(private vehicleService: VehicleService) {}

  deleteVehicle(vehicle: Vehicle, index: number, vehiclesList: Vehicle[]): void {
    this.vehicleService.deleteVehicle(vehicle).subscribe(
      (response: DeleteVehicleResponse) => {
        console.log('Vehicle deleted successfully', response);
        if (response.DicOfDT && response.DicOfDT.Vehicles) {
          vehiclesList.splice(index, 1);
        }
      },
      (error: any) => {
        console.error('Error deleting vehicle', error);
      }
    );
  }
}