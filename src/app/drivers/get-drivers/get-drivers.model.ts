import { GVARRequest, GVARResponse } from "../../gvar.model";

export interface Driver {
    PhoneNumber: string;
    DriverName:string;
    DriverID: string;
}

export interface GetDriverInformationResponse extends GVARResponse<Driver> {
  DicOfDT: {
    Driver: Driver[];
  };
}