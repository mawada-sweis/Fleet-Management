import { GVARRequest, GVARResponse } from "../../gvar.model";
import { VehicleInformation } from "../get-vehicle-information/get-vehicle-information.model";


export interface UpdateVehicleInformationResponse extends GVARResponse<VehicleInformation> {
  DicOfDT: {
    VehiclesInformations: VehicleInformation[];
  };
}

export interface UpdateVehicleInformationRequest extends GVARRequest {
    DicOfDic: {
        Tags: {
            VehicleID: string,
            DriverID: string,
            VehicleMake: string,
            VehicleModel: string,
            PurchaseDate: string,
            ID: string
        };
      };
  }