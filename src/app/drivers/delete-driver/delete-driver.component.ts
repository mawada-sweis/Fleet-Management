import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DriverService } from '../../services/driver.service';
import { Driver } from '../get-drivers/get-drivers.model';
@Component({
  selector: 'app-delete-driver',
  templateUrl: './delete-driver.component.html',
  styleUrls: ['./delete-driver.component.css']
})
export class DeleteDriverComponent  {

  constructor(
    private driverService: DriverService,
    public dialogRef: MatDialogRef<DeleteDriverComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Driver
  ) { }

  onDelete(): void {
    this.driverService.deleteDriver(this.data).subscribe(response => {
      this.dialogRef.close(response);
    });
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}