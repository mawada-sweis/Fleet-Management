import { NgModule, Component } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
//import { VehiclesComponent } from './vehicles/vehicles.component';
import { GetVehiclesComponent } from './vehicle/get-vehicles/get-vehicles.component';
import { GetVehicleInformationComponent } from './vehicle-information/get-vehicle-information/get-vehicle-information.component';
import { AddVehicleComponent } from './vehicle/add-vehicle/add-vehicle.component';
import { GetDriversComponent } from './drivers/get-drivers/get-drivers.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'vehicles', component: GetVehiclesComponent },
  { path: 'vehicle-information', component: GetVehicleInformationComponent },
  { path: 'add-vehicle', component: AddVehicleComponent },
  { path: 'drivers', component: GetDriversComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
