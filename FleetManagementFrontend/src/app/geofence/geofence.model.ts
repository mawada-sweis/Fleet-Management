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

export interface CircleGeofence {
  GeofenceID: string;
  Latitude: string;
  Radius: string;
  Longitude: string;
}

export interface PolygonGeofence {
  GeofenceID: string;
  Latitude: string;
  Longitude: string;
}

export interface RectangleGeofence {
  South: string;
  North: string;
  West: string;
  East: string;
  GeofenceID: string;
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

export interface GetCircleGeofenceResponse {
  DicOfDic: {
    Tags: {
      STS: string;
    };
  };
  DicOfDT: {
    CircleGeofence: CircleGeofence[];
  };
}

export interface GetPolygonGeofenceResponse {
  DicOfDic: {
    Tags: {
      STS: string;
    };
  };
  DicOfDT: {
    PolygonGeofence: PolygonGeofence[];
  };
}

export interface GetRectangleGeofenceResponse {
  DicOfDic: {
    Tags: {
      STS: string;
    };
  };
  DicOfDT: {
    RectangleGeofence: RectangleGeofence[];
  };
}
