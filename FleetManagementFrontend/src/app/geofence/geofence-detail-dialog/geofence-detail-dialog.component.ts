import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-geofence-detail-dialog',
  templateUrl: './geofence-detail-dialog.component.html',
  styleUrls: ['./geofence-detail-dialog.component.css'],
})
export class GeofenceDetailDialogComponent {
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public dialogRef: MatDialogRef<GeofenceDetailDialogComponent>
  ) {}

  close(): void {
    this.dialogRef.close();
  }
  getKeys(data: any): string[] {
    return Object.keys(data);
  }
}
