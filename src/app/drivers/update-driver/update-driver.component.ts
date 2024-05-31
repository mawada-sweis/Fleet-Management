import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DriverService } from '../../services/driver.service';
import { Driver } from '../get-drivers/get-drivers.model';

@Component({
  selector: 'app-update-driver',
  templateUrl: './update-driver.component.html',
  styleUrls: ['./update-driver.component.css']
})
export class UpdateDriverComponent {
  driverForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private driverService: DriverService,
    public dialogRef: MatDialogRef<UpdateDriverComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Driver
  ) {
    this.driverForm = this.fb.group({
      DriverName: [data.DriverName, Validators.required],
      PhoneNumber: [data.PhoneNumber, Validators.required],
      DriverID: [data.DriverID, Validators.required]
    });
  }

  onSubmit(): void {
    if (this.driverForm.valid) {
      this.driverService.updateDriver(this.driverForm.value).subscribe(response => {
        this.dialogRef.close(response);
      });
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}