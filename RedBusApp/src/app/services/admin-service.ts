import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { ADMIN_CONTROLLER, API_URL, RED_BUS_CONTROLLER } from '../constants/constants';
import { Observable } from 'rxjs';
import { IApiResponse } from '../models/interface/ApiResponse';
import { Bus } from '../models/class/Bus';
import { Location as RedBusLocation } from '../models/class/Location';
import { BusModel } from '../models/class/Bus.Model.';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  http = inject(HttpClient);
  addLocation(locationObj: RedBusLocation): Observable<IApiResponse> {
    return this.http.post<IApiResponse>(`${API_URL}/${RED_BUS_CONTROLLER.ADMIN}/${ADMIN_CONTROLLER.ADD_LOCATION}`, locationObj);
  }
  updateLocation(locationObj: RedBusLocation): Observable<IApiResponse> {
    return this.http.put<IApiResponse>(`${API_URL}/${RED_BUS_CONTROLLER.ADMIN}/${ADMIN_CONTROLLER.UPDATE_LOCATION}`, locationObj);
  }
  deleteLocation(locationId: string): Observable<IApiResponse> {
    return this.http.delete<IApiResponse>(`${API_URL}/${RED_BUS_CONTROLLER.ADMIN}/${ADMIN_CONTROLLER.DELETE_LOCATION}?id=${locationId}`);
  }
  addBus(busObj: BusModel): Observable<IApiResponse> {
    return this.http.post<IApiResponse>(`${API_URL}/${RED_BUS_CONTROLLER.ADMIN}/${ADMIN_CONTROLLER.ADD_BUS}`, busObj);
  }
  updateBus(busObj: Bus): Observable<IApiResponse> {
    return this.http.put<IApiResponse>(`${API_URL}/${RED_BUS_CONTROLLER.ADMIN}/${ADMIN_CONTROLLER.UPDATE_BUS}`, busObj);
  }
  deleteBus(busId: string): Observable<IApiResponse> {
    return this.http.delete<IApiResponse>(`${API_URL}/${RED_BUS_CONTROLLER.ADMIN}/${ADMIN_CONTROLLER.DELETE_BUS}?id=${busId}`);
  }
}
