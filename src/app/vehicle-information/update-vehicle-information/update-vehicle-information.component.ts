import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { VehicleInformationService } from './../../services/vehicle-information.service';
import { UpdateVehicleInformationResponse } from './update-vehicle-information.model';
import { VehicleInformation } from '../get-vehicle-information/get-vehicle-information.model';

@Component({
  selector: 'app-update-vehicle-information',
  templateUrl: './update-vehicle-information.component.html',
  styleUrls: ['./update-vehicle-information.component.css'],
})
export class UpdateVehicleInformationComponent {
  editVehicleForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private vehicleInformationService: VehicleInformationService,
    public dialogRef: MatDialogRef<UpdateVehicleInformationComponent>,
    @Inject(MAT_DIALOG_DATA) public data: VehicleInformation
  ) {
    this.editVehicleForm = this.fb.group({
      VehicleID: [data.VehicleID, Validators.required],
      DriverID: [data.DriverID, Validators.required],
      VehicleMake: [data.VehicleMake, Validators.required],
      VehicleModel: [data.VehicleModel, Validators.required],
      PurchaseDate: [data.PurchaseDate, Validators.required],
      ID: [data.ID, Validators.required],
    });
  }

  onSubmit(): void {
    if (this.editVehicleForm.valid) {
      const updatedVehicle: VehicleInformation = this.editVehicleForm.value;
      this.vehicleInformationService
        .updateVehicleInformation(updatedVehicle)
        .subscribe((response: UpdateVehicleInformationResponse) => {
          this.dialogRef.close(response.DicOfDT.VehiclesInformations[0]);
        });
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
