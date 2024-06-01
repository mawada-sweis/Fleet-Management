import { VehicleInformation } from '../get-vehicle-information/get-vehicle-information.model';
import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { VehicleInformationService } from '../../services/vehicle-information.service';

@Component({
  selector: 'app-add-vehicle-information',
  templateUrl: './add-vehicle-information.component.html',
  styleUrls: ['./add-vehicle-information.component.css'],
})
export class AddVehicleInformationComponent {
  addVehicleForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private vehicleInformationService: VehicleInformationService,
    public dialogRef: MatDialogRef<AddVehicleInformationComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.addVehicleForm = this.fb.group({
      VehicleID: ['', Validators.required],
      DriverID: ['', Validators.required],
      VehicleMake: ['', Validators.required],
      VehicleModel: ['', Validators.required],
      PurchaseDate: ['', Validators.required],
    });
  }

  onSubmit(): void {
    if (this.addVehicleForm.valid) {
      const newVehicle: VehicleInformation = this.addVehicleForm.value;
      this.vehicleInformationService
        .addVehicleInformation(newVehicle)
        .subscribe((response) => {
          this.dialogRef.close(response);
        });
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
