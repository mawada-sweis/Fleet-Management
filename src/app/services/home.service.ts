import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { VehicleInformationsResponse, SpecificVehicleInformationRequest,
   VehicleResponse } from '../home/home.model';
@Injectable({
  providedIn: 'root'
})

export class HomeService {
  private baseUrl = 'http://localhost:5294/api/vehicles';

  constructor(private http: HttpClient) { }

  getVehiclesDetails(): Observable<VehicleInformationsResponse> {
    return this.http.get<VehicleInformationsResponse>(`${this.baseUrl}/GetVehicleInformations`);
  }

  getSpecificVehicleDetails(vehicleId: string): Observable<VehicleResponse> {
    const body: SpecificVehicleInformationRequest = {
      DicOfDic: {
        Tags: {
          VehicleID: vehicleId
        }
      }
    };
    return this.http.post<VehicleResponse>(`${this.baseUrl}/GetVehicleInformation`, body);
  }
}
