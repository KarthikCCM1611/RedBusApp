import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { IApiResponse } from '../models/interface/ApiResponse';
import { API_URL, MASTER_CONTROLLER, RED_BUS_CONTROLLER } from '../constants/constants';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MasterService {
  http = inject(HttpClient);

  trendingRoutes(): Observable<IApiResponse> {
    return this.http.get<IApiResponse>(`${API_URL}/${RED_BUS_CONTROLLER.MASTER}/${MASTER_CONTROLLER.TRENDING_ROUTES}`);
  }
  searchBuses(fromLocationId: string, toLocationId: string): Observable<IApiResponse> {
    return this.http.get<IApiResponse>(`${API_URL}/${RED_BUS_CONTROLLER.MASTER}/${MASTER_CONTROLLER.SEARCH_BUSES}?fromLocationId=${fromLocationId}&&toLocationId=${toLocationId}`);
  }

  getAllLocations(): Observable<IApiResponse> {
    return this.http.get<IApiResponse>(`${API_URL}/${RED_BUS_CONTROLLER.MASTER}/${MASTER_CONTROLLER.GET_ALL_LOCATIONS}`);
  }

  getAllBuses(): Observable<IApiResponse> {
    return this.http.get<IApiResponse>(`${API_URL}/${RED_BUS_CONTROLLER.MASTER}/${MASTER_CONTROLLER.GET_ALL_BUSES}`);
  }

  getBusDetailsById(busId: string): Observable<IApiResponse> {
    return this.http.get<IApiResponse>(`${API_URL}/${RED_BUS_CONTROLLER.MASTER}/${MASTER_CONTROLLER.GET_BUS_DETAILS_BY_ID}?id=${busId}`);
  }
}
