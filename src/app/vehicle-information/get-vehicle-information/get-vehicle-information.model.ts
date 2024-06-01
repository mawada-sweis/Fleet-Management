import { GVARRequest, GVARResponse } from '../../gvar.model';

export interface VehicleInformation {
  VehicleID: string;
  DriverID: string;
  VehicleMake: string;
  VehicleModel: string;
  PurchaseDate: string;
  ID: string;
}

export interface GetVehicleInformationResponse
  extends GVARResponse<VehicleInformation> {
  DicOfDT: {
    VehiclesInformations: VehicleInformation[];
  };
}
