import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  GetRoutHistoryRequest,
  GetRoutHistoryResponse,
  RoutHistory,
} from '../rout-history/get-rout-history/get-rout-history.model';
import { GetVehicleResponse } from '../vehicle/get-vehicles/get-vehicles.model';
import {
  AddRoutHistory,
  AddRoutHistoryRequest,
  AddRoutHistoryResponse,
} from '../rout-history/add-rout-history/add-rout-history.model';

@Injectable({
  providedIn: 'root',
})
export class RoutHistoryService {
  private baseUrl = 'http://localhost:5294/api/vehicles';

  constructor(private http: HttpClient) {}

  getVehicles(): Observable<GetVehicleResponse> {
    return this.http.get<GetVehicleResponse>(`${this.baseUrl}/GetVehicles`);
  }

  getRoutHistory(routHistory: RoutHistory): Observable<GetRoutHistoryResponse> {
    const body: GetRoutHistoryRequest = {
      DicOfDic: {
        Tags: {
          VehicleID: routHistory.VehicleID,
          StartEpoch: routHistory.StartEpoch,
          EndEpoch: routHistory.EndEpoch,
        },
      },
    };
    return this.http.post<GetRoutHistoryResponse>(
      `${this.baseUrl}/GetVehicleRouteHistory`,
      body
    );
  }

  addRoutHistory(
    routHistory: AddRoutHistory
  ): Observable<AddRoutHistoryResponse> {
    const body: AddRoutHistoryRequest = {
      DicOfDic: {
        Tags: {
          VehicleID: routHistory.VehicleID,
          VehicleDirection: routHistory.VehicleDirection,
          Longitude: routHistory.Longitude,
          Latitude: routHistory.Latitude,
          Status: routHistory.Status,
          Address: routHistory.Address,
          VehicleSpeed: routHistory.VehicleSpeed,
          Epoch: routHistory.Epoch,
        },
      },
    };
    return this.http.post<AddRoutHistoryResponse>(
      `${this.baseUrl}/AddRouteHistory`,
      body
    );
  }
}
