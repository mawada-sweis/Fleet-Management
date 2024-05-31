import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { VehicleInformationService } from 'src/app/services/vehicle-information.service';

@Component({
  selector: 'app-delete-vehicle-information',
  templateUrl: './delete-vehicle-information.component.html',
  styleUrls: ['./delete-vehicle-information.component.css']
})
export class DeleteVehicleInformationComponent {
  vehicle: any;

  constructor(
    public dialogRef: MatDialogRef<DeleteVehicleInformationComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private vehicleInformationService: VehicleInformationService
  ) {
    this.vehicle = data.vehicle;
  }

  onDelete(): void {
    this.vehicleInformationService.deleteVehicleInformation(this.vehicle).subscribe(
      () => {
        this.dialogRef.close(true);
      },
      (error) => {
        console.error('Error deleting vehicle information:', error);
        this.dialogRef.close(false);
      }
    );
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }
}