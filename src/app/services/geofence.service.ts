import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GetCircleGeofenceResponse, GetGeofenceInformationResponse, GetPolygonGeofenceResponse, GetRectangleGeofenceResponse } from '../geofence/geofence.model';

@Injectable({
  providedIn: 'root',
})
export class GeofenceService {
  private baseUrl = 'http://localhost:5294/api/vehicles';

  constructor(private http: HttpClient) {}

  getGeofences(): Observable<GetGeofenceInformationResponse> {
    return this.http.get<GetGeofenceInformationResponse>(
      `${this.baseUrl}/GetGeofenceInformation`
    );
  }

  getCircleGeofence(): Observable<GetCircleGeofenceResponse> {
    return this.http.get<GetCircleGeofenceResponse>(
      `${this.baseUrl}/GetCircleGeofence`
    );
  }

  getPolygonGeofence(): Observable<GetPolygonGeofenceResponse> {
    return this.http.get<GetPolygonGeofenceResponse>(
      `${this.baseUrl}/GetPolygonGeofence`
    );
  }

  getRectangleGeofence(): Observable<GetRectangleGeofenceResponse> {
    return this.http.get<GetRectangleGeofenceResponse>(
      `${this.baseUrl}/GetRectangleGeofence`
    );
  }
}
