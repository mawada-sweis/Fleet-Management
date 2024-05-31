import { GVARRequest, GVARResponse } from "../../gvar.model";

  export interface UpdateDriverRequest extends GVARRequest {
    DicOfDic: {
        Tags: {
            DriverName: string,
            PhoneNumber: string,
            DriverID: string
        };
      };
  }