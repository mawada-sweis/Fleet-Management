import { GVARRequest, GVARResponse } from '../../gvar.model';

export interface Vehicle {
  VehicleNumber: string;
  VehicleType: string;
  VehicleID: string;
}

export interface DeleteVehicleResponse extends GVARResponse<Vehicle> {
  DicOfDT: {
    Vehicles: Vehicle[];
  };
}

export interface DeleteVehicleRequest extends GVARRequest {
  DicOfDic: {
    Tags: {
      VehicleID: string;
    };
  };
}
