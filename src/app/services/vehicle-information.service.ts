import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GetVehicleInformationResponse } from '../vehicle-information/get-vehicle-information/get-vehicle-information.model';
import {
  AddVehicleInformationRequest,
  AddVehicleInformationResponse,
} from '../vehicle-information/add-vehicle-information/add-vehicle-information.model';
import {
  UpdateVehicleInformationRequest,
  UpdateVehicleInformationResponse,
} from '../vehicle-information/update-vehicle-information/update-vehicle-information.model';
import { VehicleInformation } from '../vehicle-information/get-vehicle-information/get-vehicle-information.model';
import {
  DeleteVehicleInformationRequest,
  DeleteVehicleInformationResponse,
} from '../vehicle-information/delete-vehicle-information/delete-vehicle-information.model';

@Injectable({
  providedIn: 'root',
})
export class VehicleInformationService {
  private baseUrl = 'http://localhost:5294/api/vehicles';

  constructor(private http: HttpClient) {}

  getVehicleInformation(): Observable<GetVehicleInformationResponse> {
    return this.http.get<GetVehicleInformationResponse>(
      `${this.baseUrl}/AllVehicleInformations`
    );
  }

  addVehicleInformation(
    vehicle: VehicleInformation
  ): Observable<AddVehicleInformationResponse> {
    const body: AddVehicleInformationRequest = {
      DicOfDic: {
        Tags: {
          VehicleID: vehicle.VehicleID,
          DriverID: vehicle.DriverID,
          VehicleMake: vehicle.VehicleMake,
          VehicleModel: vehicle.VehicleModel,
          PurchaseDate: vehicle.PurchaseDate,
        },
      },
    };
    return this.http.post<AddVehicleInformationResponse>(
      `${this.baseUrl}/AddVehiclesInformation`,
      body
    );
  }

  updateVehicleInformation(
    vehicle: VehicleInformation
  ): Observable<UpdateVehicleInformationResponse> {
    const body: UpdateVehicleInformationRequest = {
      DicOfDic: {
        Tags: {
          VehicleID: vehicle.VehicleID,
          DriverID: vehicle.DriverID,
          VehicleMake: vehicle.VehicleMake,
          VehicleModel: vehicle.VehicleModel,
          PurchaseDate: vehicle.PurchaseDate,
          ID: vehicle.ID,
        },
      },
    };
    return this.http.put<UpdateVehicleInformationResponse>(
      `${this.baseUrl}/UpdateVehiclesInformation`,
      body
    );
  }

  deleteVehicleInformation(
    vehicle: VehicleInformation
  ): Observable<DeleteVehicleInformationResponse> {
    const body: DeleteVehicleInformationRequest = {
      DicOfDic: {
        Tags: {
          ID: vehicle.ID,
        },
      },
    };
    const options = {
      headers: { 'Content-Type': 'application/json' },
      body: body,
    };

    return this.http.delete<DeleteVehicleInformationResponse>(
      `${this.baseUrl}/DeleteVehiclesInformation`,
      options
    );
  }
}
