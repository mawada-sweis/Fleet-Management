// vehicle-information.model.ts
import { GVARRequest, GVARResponse } from "../gvar.model";

export interface VehiclesInformations {
  VehicleID: string;
  LastStatus: string;
  Longitude: string;
  LastDirection: string;
  Latitude: string;
  VehicleType: string;
  VehicleNumber: string;
  LastAddress: string;
}

export interface Vehicle{
  VehicleMake: string;
  LastGPSSpeed: string;
  PhoneNumber: string;
  VehicleModel: string;
  LastGPSTime: string;
  DriverName: string;
  LastPosition?: string;
}

export interface VehicleInformationsResponse extends GVARResponse<VehiclesInformations> {
  DicOfDT: {
    VehiclesInformations: VehiclesInformations[];
  };
}

export interface VehicleResponse extends GVARResponse<Vehicle> {
  DicOfDT: {
    VehiclesInformations: Vehicle[];
  };
}

export interface SpecificVehicleInformationRequest extends GVARRequest {
  DicOfDic: {
    Tags: {
      VehicleID: string;
    };
  };
}

