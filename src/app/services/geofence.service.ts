import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GetGeofenceInformationResponse } from '../geofence/geofence.model';

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
}
