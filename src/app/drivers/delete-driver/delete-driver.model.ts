import { GVARRequest, GVARResponse } from '../../gvar.model';

export interface DeleteDriverRequest extends GVARRequest {
  DicOfDic: {
    Tags: {
      DriverID: string;
    };
  };
}
