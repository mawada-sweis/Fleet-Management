import { GVARRequest, GVARResponse } from '../../gvar.model';
import { VehicleInformation } from '../get-vehicle-information/get-vehicle-information.model';

export interface AddVehicleInformationResponse
  extends GVARResponse<VehicleInformation> {
  DicOfDT: {
    VehiclesInformations: VehicleInformation[];
  };
}

export interface AddVehicleInformationRequest extends GVARRequest {
  DicOfDic: {
    Tags: {
      VehicleID: string;
      DriverID: string;
      VehicleMake: string;
      VehicleModel: string;
      PurchaseDate: string;
    };
  };
}
