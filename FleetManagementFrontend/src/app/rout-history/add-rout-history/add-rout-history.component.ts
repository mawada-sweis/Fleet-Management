import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RoutHistoryService } from '../../services/rout-history.service';
import {
  Vehicle,
  GetVehicleResponse,
} from '../../vehicle/get-vehicles/get-vehicles.model';
import {
  AddRoutHistory,
  AddRoutHistoryResponse,
} from './add-rout-history.model';

@Component({
  selector: 'app-add-rout-history',
  templateUrl: './add-rout-history.component.html',
  styleUrls: ['./add-rout-history.component.css'],
})
export class AddRoutHistoryComponent implements OnInit {
  vehicles: Vehicle[] = [];
  addRouteHistoryForm: FormGroup;
  errorMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private routHistoryService: RoutHistoryService,
    public dialogRef: MatDialogRef<AddRoutHistoryComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.addRouteHistoryForm = this.fb.group({
      VehicleID: ['', Validators.required],
      VehicleDirection: ['', Validators.required],
      Longitude: ['', Validators.required],
      Latitude: ['', Validators.required],
      Status: ['', Validators.required],
      Address: ['', Validators.required],
      VehicleSpeed: ['', Validators.required],
      Epoch: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    this.loadVehicles();
  }

  loadVehicles(): void {
    this.routHistoryService.getVehicles().subscribe(
      (response: GetVehicleResponse) => {
        this.vehicles = response.DicOfDT.Vehicles;
      },
      (error) => {
        this.errorMessage = 'Failed to load vehicles';
      }
    );
  }

  onSubmit(): void {
    if (this.addRouteHistoryForm.valid) {
      this.routHistoryService
        .addRoutHistory(this.addRouteHistoryForm.value)
        .subscribe(
          (response: AddRoutHistoryResponse) => {
            this.dialogRef.close(response.DicOfDT.RouteHistory);
          },
          (error) => {
            this.errorMessage = 'Failed to add route history';
          }
        );
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
