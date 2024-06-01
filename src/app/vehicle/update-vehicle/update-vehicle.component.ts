import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { VehicleService } from '../../services/vehicle.service';
import {
  Vehicle,
  UpdateVehicleResponse,
  UpdateVehicleRequest,
} from './update-vehicle.model';

@Component({
  selector: 'app-update-vehicle',
  templateUrl: './update-vehicle.component.html',
  styleUrls: ['./update-vehicle.component.css'],
})
export class UpdateVehicleComponent {
  constructor(
    public dialogRef: MatDialogRef<UpdateVehicleComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Vehicle,
    private vehicleService: VehicleService
  ) {}

  updateVehicle(): void {
    this.vehicleService.updateVehicle(this.data).subscribe(
      (response: UpdateVehicleResponse) => {
        console.log('Vehicle updated successfully', response);
        this.dialogRef.close(this.data);
      },
      (error: any) => {
        console.error('Error updating vehicle', error);
      }
    );
  }

  close(): void {
    this.dialogRef.close();
  }
}
