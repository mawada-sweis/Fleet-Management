import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DriverService } from '../../services/driver.service';

@Component({
  selector: 'app-add-driver',
  templateUrl: './add-driver.component.html',
  styleUrls: ['./add-driver.component.css'],
})
export class AddDriverComponent {
  driverForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private driverService: DriverService,
    public dialogRef: MatDialogRef<AddDriverComponent>
  ) {
    this.driverForm = this.fb.group({
      DriverName: ['', Validators.required],
      PhoneNumber: ['', Validators.required],
    });
  }

  onSubmit(): void {
    if (this.driverForm.valid) {
      this.driverService
        .addDriver(this.driverForm.value)
        .subscribe((response) => {
          this.dialogRef.close(response);
        });
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
