import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';

import {
  MatDialogModule,
  MAT_DIALOG_DATA,
  MatDialogRef,
} from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';

import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { HomeComponent } from './home/home.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { AddVehicleComponent } from './vehicle/add-vehicle/add-vehicle.component';
import { UpdateVehicleComponent } from './vehicle/update-vehicle/update-vehicle.component';
import { GetVehiclesComponent } from './vehicle/get-vehicles/get-vehicles.component';
import { DeleteVehicleComponent } from './vehicle/delete-vehicle/delete-vehicle.component';
import { AddVehicleInformationComponent } from './vehicle-information/add-vehicle-information/add-vehicle-information.component';
import { UpdateVehicleInformationComponent } from './vehicle-information/update-vehicle-information/update-vehicle-information.component';
import { DeleteVehicleInformationComponent } from './vehicle-information/delete-vehicle-information/delete-vehicle-information.component';
import { GetVehicleInformationComponent } from './vehicle-information/get-vehicle-information/get-vehicle-information.component';
import { GetDriversComponent } from './drivers/get-drivers/get-drivers.component';
import { UpdateDriverComponent } from './drivers/update-driver/update-driver.component';
import { AddDriverComponent } from './drivers/add-driver/add-driver.component';
import { DeleteDriverComponent } from './drivers/delete-driver/delete-driver.component';
import { GetRoutHistoryComponent } from './rout-history/get-rout-history/get-rout-history.component';
import { AddRoutHistoryComponent } from './rout-history/add-rout-history/add-rout-history.component';
import { GeofenceListComponent } from './geofence/geofence-list/geofence-list.component';
import { GeofenceDetailDialogComponent } from './geofence/geofence-detail-dialog/geofence-detail-dialog.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    SidebarComponent,
    AddVehicleComponent,
    UpdateVehicleComponent,
    GetVehiclesComponent,
    DeleteVehicleComponent,
    AddVehicleInformationComponent,
    UpdateVehicleInformationComponent,
    DeleteVehicleInformationComponent,
    GetVehicleInformationComponent,
    GetDriversComponent,
    UpdateDriverComponent,
    AddDriverComponent,
    DeleteDriverComponent,
    GetRoutHistoryComponent,
    AddRoutHistoryComponent,
    GeofenceListComponent,
    GeofenceDetailDialogComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    MatFormFieldModule,
    MatDialogModule,
    MatTableModule,
    MatButtonModule,
    MatInputModule,
    MatSelectModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    FormsModule,
  ],
  providers: [
    { provide: MAT_DIALOG_DATA, useValue: {} },
    { provide: MatDialogRef, useValue: {} },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
