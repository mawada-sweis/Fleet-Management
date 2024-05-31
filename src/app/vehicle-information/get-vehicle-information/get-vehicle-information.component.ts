import { VehicleInformation } from './get-vehicle-information.model';
import { Component, OnInit } from '@angular/core';
import { VehicleInformationService } from 'src/app/services/vehicle-information.service';
import { GetVehicleInformationResponse } from './get-vehicle-information.model';
import { MatDialog } from '@angular/material/dialog';
import { AddVehicleInformationComponent } from '../add-vehicle-information/add-vehicle-information.component';
import { UpdateVehicleInformationComponent } from '../update-vehicle-information/update-vehicle-information.component';
import { DeleteVehicleInformationComponent } from '../delete-vehicle-information/delete-vehicle-information.component';

@Component({
  selector: 'app-get-vehicle-information',
  templateUrl: './get-vehicle-information.component.html',
  styleUrls: ['./get-vehicle-information.component.css']
})

export class GetVehicleInformationComponent implements OnInit {
  VehiclesInformationsData: VehicleInformation[] = [];
  showTable: boolean = false;
  errorMessage: string | null = null;
  editIndex: number | null = null;

  constructor(private vehicleInformationService: VehicleInformationService, public dialog: MatDialog) {}

  ngOnInit(): void {this.loadVehicles();}

  loadVehicles(): void {
    this.vehicleInformationService.getVehicleInformation().subscribe(
      (response: GetVehicleInformationResponse) => {
        if (response.DicOfDic['Tags']['STS'] === '1') {
          this.VehiclesInformationsData = response.DicOfDT.VehiclesInformations.map(vehicle => {
            return {
              ...vehicle,
              PurchaseDate: this.convertEpochToDatetime(vehicle.PurchaseDate)
            };
          });
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

  convertEpochToDatetime(epoch: string): string {
    const date = new Date(parseInt(epoch, 10) * 1000);
    
    const year = new Intl.DateTimeFormat('en', { year: 'numeric', timeZone: 'UTC' }).format(date);
    const month = new Intl.DateTimeFormat('en', { month: '2-digit', timeZone: 'UTC' }).format(date);
    const day = new Intl.DateTimeFormat('en', { day: '2-digit', timeZone: 'UTC' }).format(date);
    const hour = new Intl.DateTimeFormat('en', { hour: '2-digit', hour12: false, timeZone: 'UTC' }).format(date);
    const minute = new Intl.DateTimeFormat('en', { minute: '2-digit', timeZone: 'UTC' }).format(date);
    const second = new Intl.DateTimeFormat('en', { second: '2-digit', timeZone: 'UTC' }).format(date);
  
    return `${year}-${month}-${day} ${hour}:${minute}:${second}`;
  }

  openAddDialog(): void {
    const dialogRef = this.dialog.open(AddVehicleInformationComponent, {
      width: '400px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.VehiclesInformationsData.push(result);
        this.loadVehicles();
      }
    });
  }

  openEditDialog(vehicle: VehicleInformation, index: number): void {
    this.editIndex = index;
    const dialogRef = this.dialog.open(UpdateVehicleInformationComponent, {
      width: '400px',
      data: { ...vehicle, ID: vehicle.ID }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result && this.editIndex !== null) {
        this.VehiclesInformationsData[this.editIndex] = result;
        this.editIndex = null;
        this.loadVehicles();
      }
    });
  }

  openDeleteDialog(vehicle: VehicleInformation, index: number): void {
    const dialogRef = this.dialog.open(DeleteVehicleInformationComponent, {
      width: '400px',
      data: { vehicle }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.VehiclesInformationsData.splice(index, 1);
        this.loadVehicles();
      }
    });
  }
}