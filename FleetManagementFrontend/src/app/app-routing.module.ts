import { GetRoutHistoryComponent } from './rout-history/get-rout-history/get-rout-history.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { GetVehiclesComponent } from './vehicle/get-vehicles/get-vehicles.component';
import { GetVehicleInformationComponent } from './vehicle-information/get-vehicle-information/get-vehicle-information.component';
import { AddVehicleComponent } from './vehicle/add-vehicle/add-vehicle.component';
import { GetDriversComponent } from './drivers/get-drivers/get-drivers.component';
import { GeofenceListComponent } from './geofence/geofence-list/geofence-list.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'vehicles', component: GetVehiclesComponent },
  { path: 'vehicle-information', component: GetVehicleInformationComponent },
  { path: 'add-vehicle', component: AddVehicleComponent },
  { path: 'drivers', component: GetDriversComponent },
  { path: 'rout-history', component: GetRoutHistoryComponent },
  { path: 'geofence', component: GeofenceListComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
