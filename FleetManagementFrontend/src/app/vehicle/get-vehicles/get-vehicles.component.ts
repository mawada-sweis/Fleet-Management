import { Component, OnInit } from '@angular/core';
import { VehicleService } from './../../services/vehicle.service';
import { Vehicle, GetVehicleResponse } from './get-vehicles.model';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { UpdateVehicleComponent } from '../update-vehicle/update-vehicle.component';

@Component({
  selector: 'app-get-vehicles',
  templateUrl: './get-vehicles.component.html',
  styleUrls: ['./get-vehicles.component.css'],
})
export class GetVehiclesComponent implements OnInit {
  VehiclesInformationsData: Vehicle[] = [];
  showTable: boolean = false;
  errorMessage: string | null = null;
  editIndex: number | null = null;

  constructor(
    private vehicleService: VehicleService,
    public dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadVehicles();
  }

  loadVehicles(): void {
    this.vehicleService.getVehicles().subscribe(
      (response: GetVehicleResponse) => {
        if (response.DicOfDic['Tags']['STS'] === '1') {
          this.VehiclesInformationsData = response.DicOfDT.Vehicles.sort(
            (a, b) => {
              return a.VehicleID.localeCompare(b.VehicleID);
            }
          );
          this.showTable = true;
          this.errorMessage = null;
        } else {
          this.showTable = false;
          this.errorMessage = 'There is something wrong.';
        }
      },
      (error) => {
        this.showTable = false;
        this.errorMessage = 'Error fetching data from server';
        console.error(error);
      }
    );
  }
  onVehicleAdded(newVehicle: Vehicle): void {
    this.VehiclesInformationsData.push(newVehicle);
    this.VehiclesInformationsData.sort((a, b) =>
      a.VehicleID.localeCompare(b.VehicleID)
    );
  }
  openEditDialog(vehicle: any, index: number): void {
    this.editIndex = index;
    const dialogRef = this.dialog.open(UpdateVehicleComponent, {
      data: { ...vehicle },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        const updatedVehicle = result.DicOfDT?.Vehicles?.find(
          (v: Vehicle) => v.VehicleID === vehicle.VehicleID
        );

        if (updatedVehicle && this.editIndex !== null) {
          this.VehiclesInformationsData[this.editIndex] = {
            ...updatedVehicle,
          };
          this.VehiclesInformationsData.sort((a, b) =>
            a.VehicleID.localeCompare(b.VehicleID)
          );
          this.loadVehicles();
          this.editIndex = null;
        }
      }
    });
  }

  deleteVehicle(vehicle: Vehicle, index: number): void {
    this.vehicleService.deleteVehicle(vehicle).subscribe(
      (response: GetVehicleResponse) => {
        console.log('Vehicle deleted successfully', response);
        if (response.DicOfDT && response.DicOfDT.Vehicles) {
          this.VehiclesInformationsData.splice(index, 1);
          this.VehiclesInformationsData.sort((a, b) =>
            a.VehicleID.localeCompare(b.VehicleID)
          );
        }
      },
      (error: any) => {
        console.error('Error deleting vehicle', error);
      }
    );
  }
}
