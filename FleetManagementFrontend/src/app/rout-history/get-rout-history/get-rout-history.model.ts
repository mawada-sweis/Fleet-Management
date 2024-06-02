import { GVARRequest, GVARResponse } from '../../gvar.model';

export interface RoutHistory {
  VehicleID: string;
  StartEpoch: string;
  EndEpoch: string;
  GPSTime: string;
  VehicleDirection: string;
  Longitude: string;
  Latitude: string;
  Status: string;
  Address: string;
  GPSSpeed: string;
  VehicleNumber: string;
}

export interface GetRoutHistoryRequest extends GVARRequest {
  DicOfDic: {
    Tags: {
      VehicleID: string;
      StartEpoch: string;
      EndEpoch: string;
    };
  };
}

export interface GetRoutHistoryResponse extends GVARResponse<RoutHistory> {
  DicOfDT: {
    RouteHistory: RoutHistory[];
  };
}

export interface Vehicle {
  VehicleID: string;
}

export interface GetVehicleResponse extends GVARResponse<Vehicle> {
  DicOfDT: {
    Vehicles: Vehicle[];
  };
}
