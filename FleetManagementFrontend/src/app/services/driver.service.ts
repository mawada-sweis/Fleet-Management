import { DeleteDriverRequest } from './../drivers/delete-driver/delete-driver.model';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  Driver,
  GetDriverInformationResponse,
} from '../drivers/get-drivers/get-drivers.model';
import { AddDriverRequest } from '../drivers/add-driver/add-driver.model';
import { UpdateDriverRequest } from '../drivers/update-driver/update-driver.model';

@Injectable({
  providedIn: 'root',
})
export class DriverService {
  private baseUrl = 'http://localhost:5294/api/vehicles';
  constructor(private http: HttpClient) {}

  getDriversDetails(): Observable<GetDriverInformationResponse> {
    return this.http.get<GetDriverInformationResponse>(
      `${this.baseUrl}/GetDriverInformation`
    );
  }

  addDriver(driver: Driver): Observable<GetDriverInformationResponse> {
    const body: AddDriverRequest = {
      DicOfDic: {
        Tags: {
          DriverName: driver.DriverName,
          PhoneNumber: driver.PhoneNumber,
        },
      },
    };
    return this.http.post<GetDriverInformationResponse>(
      `${this.baseUrl}/AddDriver`,
      body
    );
  }

  updateDriver(driver: Driver): Observable<GetDriverInformationResponse> {
    const body: UpdateDriverRequest = {
      DicOfDic: {
        Tags: {
          DriverName: driver.DriverName,
          PhoneNumber: driver.PhoneNumber,
          DriverID: driver.DriverID,
        },
      },
    };
    return this.http.put<GetDriverInformationResponse>(
      `${this.baseUrl}/UpdateDriver`,
      body
    );
  }

  deleteDriver(driver: Driver): Observable<GetDriverInformationResponse> {
    const body: DeleteDriverRequest = {
      DicOfDic: {
        Tags: {
          DriverID: driver.DriverID,
        },
      },
    };
    const options = {
      headers: { 'Content-Type': 'application/json' },
      body: body,
    };

    return this.http.delete<GetDriverInformationResponse>(
      `${this.baseUrl}/DeleteDriver`,
      options
    );
  }
}
