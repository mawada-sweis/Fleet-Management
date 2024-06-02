import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  AddVehicleRequest,
  AddVehicleResponse,
} from '../vehicle/add-vehicle/add-vehicle.model';
import { DeleteVehicleRequest } from '../vehicle/delete-vehicle/delete-vehicle.model';
import { UpdateVehicleRequest } from '../vehicle/update-vehicle/update-vehicle.model';
import { Vehicle } from '../vehicle/get-vehicles/get-vehicles.model';

@Injectable({
  providedIn: 'root',
})
export class VehicleService {
  private baseUrl = 'http://localhost:5294/api/vehicles';

  constructor(private http: HttpClient) {}

  getVehicles(): Observable<AddVehicleResponse> {
    return this.http.get<AddVehicleResponse>(`${this.baseUrl}/GetVehicles`);
  }

  addVehicle(vehicle: Vehicle): Observable<AddVehicleResponse> {
    const body: AddVehicleRequest = {
      DicOfDic: {
        Tags: {
          VehicleNumber: vehicle.VehicleNumber,
          VehicleType: vehicle.VehicleType,
        },
      },
    };
    return this.http.post<AddVehicleResponse>(
      `${this.baseUrl}/AddVehicle`,
      body
    );
  }

  updateVehicle(vehicle: Vehicle): Observable<AddVehicleResponse> {
    const body: UpdateVehicleRequest = {
      DicOfDic: {
        Tags: {
          VehicleNumber: vehicle.VehicleNumber,
          VehicleType: vehicle.VehicleType,
          VehicleID: vehicle.VehicleID,
        },
      },
    };
    return this.http.put<AddVehicleResponse>(
      `${this.baseUrl}/UpdateVehicle`,
      body
    );
  }

  deleteVehicle(vehicle: Vehicle): Observable<AddVehicleResponse> {
    const body: DeleteVehicleRequest = {
      DicOfDic: {
        Tags: {
          VehicleID: vehicle.VehicleID,
        },
      },
    };
    const options = {
      headers: { 'Content-Type': 'application/json' },
      body: body,
    };

    return this.http.delete<AddVehicleResponse>(
      `${this.baseUrl}/DeleteVehicle`,
      options
    );
  }
}
