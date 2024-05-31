import { GVARRequest, GVARResponse } from "../../gvar.model";

export interface Vehicle {
  VehicleNumber: string;
  VehicleType: string;
  VehicleID: string;
}

export interface GetVehicleResponse extends GVARResponse<Vehicle> {
  DicOfDT: {
    Vehicles: Vehicle[];
  };
}