export interface Geofence {
  GeofenceID: string;
  GeofenceType: string;
  AddedDate: string;
  StrockColor: string;
  StrockOpacity: string;
  StrockWeight: string;
  FillColor: string;
  FillOpacity: string;
}

export interface GetGeofenceInformationResponse {
  DicOfDic: {
    Tags: {
      STS: string;
    };
  };
  DicOfDT: {
    Geofences: Geofence[];
  };
}
