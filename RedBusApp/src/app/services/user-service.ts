import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { API_URL, RED_BUS_CONTROLLER, USER_CONTROLLER } from '../constants/constants';
import { IApiResponse } from '../models/interface/ApiResponse';
import { Booking } from '../models/class/Booking';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  http = inject(HttpClient);

  getAllBookingsByUserId(userId: string): Observable<IApiResponse> {
    return this.http.get<IApiResponse>(`${API_URL}/${RED_BUS_CONTROLLER.USER}/${USER_CONTROLLER.GET_ALL_BOOKINGS_BY_USER_ID}?userId=${userId}`);
  }

  createBooking(bookingObj: Booking): Observable<IApiResponse> {
    return this.http.post<IApiResponse>(`${API_URL}/${RED_BUS_CONTROLLER.USER}/${USER_CONTROLLER.CREATE_BOOKING}`, bookingObj);
  }

  cancelBooking(bookingId: string): Observable<IApiResponse> {
    return this.http.delete<IApiResponse>(`${API_URL}/${RED_BUS_CONTROLLER.USER}/${USER_CONTROLLER.CANCEL_BOOKING}?bookingId=${bookingId}`);
  }
}
