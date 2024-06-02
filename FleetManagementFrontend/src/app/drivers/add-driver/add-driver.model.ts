import { GVARRequest, GVARResponse } from '../../gvar.model';
import { Driver } from '../get-drivers/get-drivers.model';

export interface AddDriverRequest extends GVARRequest {
  DicOfDic: {
    Tags: {
      DriverName: string;
      PhoneNumber: string;
    };
  };
}
