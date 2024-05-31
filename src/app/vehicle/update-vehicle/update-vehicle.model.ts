import { GVARRequest, GVARResponse } from "../../gvar.model";

export interface Vehicle {
  VehicleNumber: string;
  VehicleType: string;
  VehicleID: string;
}

export interface UpdateVehicleResponse extends GVARResponse<Vehicle> {
  DicOfDT: {
    Vehicles: Vehicle[];
  };
}

export interface UpdateVehicleRequest extends GVARRequest {
  DicOfDic: {
    Tags: {
      VehicleNumber: string,
      VehicleType: string,
      VehicleID: string
    };
  };
}