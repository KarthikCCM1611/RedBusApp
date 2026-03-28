import { Component, inject, OnInit, signal, ViewChild } from '@angular/core';
import { Toast } from '../../shared/reusableComponents/toast/toast';
import { ToastType } from '../../models/toast.model.ts';
import { UserService } from '../../services/user-service';
import { IApiResponse } from '../../models/interface/ApiResponse';
import { BookingModel } from '../../models/class/Booking,Model';
import { AuthService } from '../../services/auth-service';

@Component({
  selector: 'app-booking-list',
  imports: [Toast],
  templateUrl: './booking-list.html',
  styleUrl: './booking-list.css',
})
export class BookingList {
  authSrc = inject(AuthService);
  userSrc = inject(UserService);

  userId = signal<string>('');
  bookingList = signal<BookingModel[]>([]);
  isSearching = signal<boolean>(true);
  errorMsg = signal<string>('');

  @ViewChild('toast') toast!: Toast;
  toastType = signal<ToastType>('info');
  delay = signal<number>(2000);
  constructor() {
    const userId = this.authSrc.userId();
    if (userId) {
      this.userId.set(userId);
      this.getAllBookings();
    }
  }

  getAllBookings() {
    this.userSrc.getAllBookingsByUserId(this.userId()).subscribe({
      next: (res: IApiResponse) => {
        if (res.statusCode.toString() === "200") {
          this.toastType.set('success');
          this.toast.show(res.statusMessage);
          this.bookingList.set(res.data);
        }
        else {
          if (res.statusCode.toString() === '404') {
            this.bookingList.set([]);
          }
          this.toastType.set('danger');
          this.toast.show(res.statusMessage);
        }
        this.isSearching.set(false);
      },
      error: (error: IApiResponse) => {
        this.toastType.set('danger');
        this.toast.show(error.statusMessage);
        this.isSearching.set(false);
      }
    })
  }

  cancelBooking(booking: BookingModel) {
    const confirmStatus = confirm("Do you want to cancel the booking?");
    if (!confirmStatus) {
      return;
    }
    this.userSrc.cancelBooking(booking.id).subscribe({
      next: (res: IApiResponse) => {
        if (res.statusCode.toString() === "200") {
          this.toastType.set('success');
          this.toast.show(res.statusMessage);
          this.getAllBookings();
        }
        else {
          this.toastType.set('danger');
          this.toast.show(res.statusMessage);
        }
        this.isSearching.set(false);
      },
      error: (error: IApiResponse) => {
        this.toastType.set('danger');
        this.toast.show(error.statusMessage);
        this.isSearching.set(false);
      }
    })
  }
}
