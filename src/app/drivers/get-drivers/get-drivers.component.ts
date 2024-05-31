import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DriverService } from '../../services/driver.service';
import { Driver, GetDriverInformationResponse } from './get-drivers.model';
import { AddDriverComponent } from '../add-driver/add-driver.component';
import { UpdateDriverComponent } from '../update-driver/update-driver.component';
import { DeleteDriverComponent } from '../delete-driver/delete-driver.component';

@Component({
  selector: 'app-get-drivers',
  templateUrl: './get-drivers.component.html',
  styleUrls: ['./get-drivers.component.css'],
})
export class GetDriversComponent implements OnInit {
  drivers: Driver[] = [];
  showTable: boolean = false;
  errorMessage: string | null = null;
  constructor(private driverService: DriverService, public dialog: MatDialog) {}

  ngOnInit(): void {
    this.getDriversDetails();
  }

  getDriversDetails(): void {
    this.driverService
      .getDriversDetails()
      .subscribe((response: GetDriverInformationResponse) => {
        if (response.DicOfDic['Tags']['STS'] === '1') {
        this.drivers = response.DicOfDT.Driver;
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
    const dialogRef = this.dialog.open(AddDriverComponent, {
      width: '400px',
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.drivers.push(result);
        this.getDriversDetails();
      }
    });
  }

  openUpdateDialog(driver: GetDriverInformationResponse): void {
    const dialogRef = this.dialog.open(UpdateDriverComponent, {
      width: '250px',
      data: driver,
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.getDriversDetails();
      }
    });
  }

  openDeleteDialog(driver: GetDriverInformationResponse): void {
    const dialogRef = this.dialog.open(DeleteDriverComponent, {
      width: '250px',
      data: driver,
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.getDriversDetails();
      }
    });
  }
}
