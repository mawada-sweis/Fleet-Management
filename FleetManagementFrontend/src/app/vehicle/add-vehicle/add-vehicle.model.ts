import { GVARRequest, GVARResponse } from '../../gvar.model';

export interface Vehicle {
  VehicleNumber: string;
  VehicleType: string;
  VehicleID: string;
}

export interface AddVehicleRequest extends GVARRequest {
  DicOfDic: {
    Tags: {
      VehicleNumber: string;
      VehicleType: string;
    };
  };
}

export interface AddVehicleResponse extends GVARResponse<Vehicle> {
  DicOfDT: {
    Vehicles: Vehicle[];
  };
}
