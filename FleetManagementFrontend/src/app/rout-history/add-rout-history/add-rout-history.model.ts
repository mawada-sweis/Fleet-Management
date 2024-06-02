import { GVARRequest, GVARResponse } from '../../gvar.model';

export interface AddRoutHistory {
  VehicleID: string;
  VehicleDirection: string;
  Longitude: string;
  Latitude: string;
  Status: string;
  Address: string;
  VehicleSpeed: string;
  Epoch: string;
}

export interface AddRoutHistoryRequest extends GVARRequest {
  DicOfDic: {
    Tags: {
      VehicleID: string;
      VehicleDirection: string;
      Longitude: string;
      Latitude: string;
      Status: string;
      Address: string;
      VehicleSpeed: string;
      Epoch: string;
    };
  };
}

export interface AddRoutHistoryResponse extends GVARResponse<AddRoutHistory> {
  DicOfDT: {
    RouteHistory: AddRoutHistory[];
  };
}
