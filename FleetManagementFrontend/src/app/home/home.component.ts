import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { HomeService } from '../services/home.service';
import {
  VehicleInformationsResponse,
  VehiclesInformations,
  Vehicle,
  VehicleResponse,
} from './home.model';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  @ViewChild('dialogTemplate') dialogTemplate!: TemplateRef<any>;

  VehiclesInformationsData: VehiclesInformations[] = [];
  VehicleInformationData: Vehicle | null = null;
  errorMessage: string | null = null;
  showTable: boolean = false;
  constructor(private dataService: HomeService, public dialog: MatDialog) {}

  ngOnInit(): void {
    this.dataService.getVehiclesDetails().subscribe(
      (response: VehicleInformationsResponse) => {
        if (response.DicOfDic['Tags']['STS'] === '1') {
          this.VehiclesInformationsData = response.DicOfDT.VehiclesInformations;
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

  openDialog(vehicleId: string): void {
    this.dataService.getSpecificVehicleDetails(vehicleId).subscribe(
      (vehicleDetails: VehicleResponse) => {
        if (
          vehicleDetails.DicOfDic['Tags']['STS'] === '1' &&
          vehicleDetails.DicOfDT &&
          vehicleDetails.DicOfDT.VehiclesInformations &&
          vehicleDetails.DicOfDT.VehiclesInformations.length > 0
        ) {
          this.VehicleInformationData =
            vehicleDetails.DicOfDT.VehiclesInformations[0];
          this.dialog.open(this.dialogTemplate, {
            data: this.VehicleInformationData,
          });
        } else {
          this.errorMessage =
            'No details available or there is something wrong.';
          console.log('No details available or there is something wrong.');
        }
      },
      (error) => {
        this.errorMessage = 'Error fetching vehicle details';
        console.error('Error fetching vehicle details', error);
      }
    );
  }
}
