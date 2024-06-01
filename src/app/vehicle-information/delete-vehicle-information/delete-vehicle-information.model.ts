import { GVARRequest, GVARResponse } from '../../gvar.model';
import { VehicleInformation } from '../get-vehicle-information/get-vehicle-information.model';

export interface DeleteVehicleInformationResponse
  extends GVARResponse<VehicleInformation> {
  DicOfDT: {
    VehiclesInformations: VehicleInformation[];
  };
}

export interface DeleteVehicleInformationRequest extends GVARRequest {
  DicOfDic: {
    Tags: {
      ID: string;
    };
  };
}
