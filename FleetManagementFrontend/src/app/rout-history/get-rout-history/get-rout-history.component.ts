import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RoutHistoryService } from '../../services/rout-history.service';
import { VehicleService } from '../../services/vehicle.service';
import { RoutHistory, GetRoutHistoryResponse } from './get-rout-history.model';
import { Vehicle, GetVehicleResponse } from './get-rout-history.model';
import { MatDialog } from '@angular/material/dialog';
import { AddRoutHistoryComponent } from '../add-rout-history/add-rout-history.component';
import { AddRoutHistory } from '../add-rout-history/add-rout-history.model';

@Component({
  selector: 'app-get-rout-history',
  templateUrl: './get-rout-history.component.html',
  styleUrls: ['./get-rout-history.component.css'],
})
export class GetRoutHistoryComponent implements OnInit {
  vehicles: Vehicle[] = [];
  routeHistoryForm: FormGroup;
  routeHistories: RoutHistory[] = [];
  errorMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private routHistoryService: RoutHistoryService,
    private vehicleService: VehicleService,
    public dialog: MatDialog
  ) {
    this.routeHistoryForm = this.fb.group({
      VehicleID: ['', Validators.required],
      StartEpoch: ['', Validators.required],
      EndEpoch: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    this.loadVehicles();
  }

  loadVehicles(): void {
    this.vehicleService.getVehicles().subscribe(
      (response: GetVehicleResponse) => {
        this.vehicles = response.DicOfDT.Vehicles;
      },
      (error) => {
        this.errorMessage = 'Failed to load vehicles';
      }
    );
  }

  onSubmit(): void {
    if (this.routeHistoryForm.valid) {
      const selectedVehicleID = this.routeHistoryForm.get('VehicleID')?.value;
      this.routHistoryService
        .getRoutHistory(this.routeHistoryForm.value)
        .subscribe(
          (response: GetRoutHistoryResponse) => {
            if (response && response.DicOfDT && response.DicOfDT.RouteHistory) {
              this.routeHistories = response.DicOfDT.RouteHistory.map(
                (history) => ({
                  VehicleID: selectedVehicleID,
                  StartEpoch: this.routeHistoryForm.get('StartEpoch')?.value,
                  EndEpoch: this.routeHistoryForm.get('EndEpoch')?.value,
                  GPSTime: this.convertEpochToDatetime(history.GPSTime),
                  VehicleDirection: history.VehicleDirection,
                  Longitude: history.Longitude,
                  Latitude: history.Latitude,
                  Status: history.Status,
                  Address: history.Address,
                  GPSSpeed: history.GPSSpeed,
                  VehicleNumber: history.VehicleNumber,
                })
              ).sort(
                (a, b) =>
                  new Date(a.GPSTime).getTime() - new Date(b.GPSTime).getTime()
              );
            } else {
              this.errorMessage = 'No route history found';
            }
            this.errorMessage = null;
          },
          (error) => {
            console.error('Error loading route history:', error);
            this.errorMessage = 'Failed to load route history';
          }
        );
    }
  }

  convertEpochToDatetime(epoch: string): string {
    const date = new Date(parseInt(epoch, 10) * 1000);

    const year = new Intl.DateTimeFormat('en', {
      year: 'numeric',
      timeZone: 'UTC',
    }).format(date);
    const month = new Intl.DateTimeFormat('en', {
      month: '2-digit',
      timeZone: 'UTC',
    }).format(date);
    const day = new Intl.DateTimeFormat('en', {
      day: '2-digit',
      timeZone: 'UTC',
    }).format(date);
    const hour = new Intl.DateTimeFormat('en', {
      hour: '2-digit',
      hour12: false,
      timeZone: 'UTC',
    }).format(date);
    const minute = new Intl.DateTimeFormat('en', {
      minute: '2-digit',
      timeZone: 'UTC',
    }).format(date);
    const second = new Intl.DateTimeFormat('en', {
      second: '2-digit',
      timeZone: 'UTC',
    }).format(date);

    return `${year}-${month}-${day} ${hour}:${minute}:${second}`;
  }

  openAddDialog(): void {
    const dialogRef = this.dialog.open(AddRoutHistoryComponent, {
      width: '400px',
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        const formattedResults = result.map((history: AddRoutHistory) => ({
          VehicleID: history.VehicleID,
          StartEpoch: '',
          EndEpoch: '',
          GPSTime: this.convertEpochToDatetime(history.Epoch),
          VehicleDirection: history.VehicleDirection,
          Longitude: history.Longitude,
          Latitude: history.Latitude,
          Status: history.Status,
          Address: history.Address,
          GPSSpeed: history.VehicleSpeed,
          VehicleNumber: '',
        }));

        this.routeHistories = [
          ...this.routeHistories,
          ...formattedResults,
        ].sort(
          (a, b) =>
            new Date(a.GPSTime).getTime() - new Date(b.GPSTime).getTime()
        );
      }
    });
  }
}
