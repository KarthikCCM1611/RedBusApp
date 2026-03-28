import { Component, inject, signal, ViewChild } from '@angular/core';
import { MasterService } from '../../services/master-service';
import { ActivatedRoute } from '@angular/router';
import { Booking } from '../../models/class/Booking';
import { Toast } from '../../shared/reusableComponents/toast/toast';
import { ToastType } from '../../models/toast.model.ts';
import { IApiResponse } from '../../models/interface/ApiResponse';
import { BusModel } from '../../models/class/Bus.Model.';
import { DatePipe } from '@angular/common';
import { UserService } from '../../services/user-service';
import { AuthService } from '../../services/auth-service';

@Component({
  selector: 'app-bus-booking',
  imports: [Toast, DatePipe],
  templateUrl: './bus-booking.html',
  styleUrl: './bus-booking.css',
})
export class BusBooking {
  masterSrc = inject(MasterService);
  authSrc = inject(AuthService);
  userService = inject(UserService);
  activatedRoute = inject(ActivatedRoute);

  userId = signal<string>('');
  busId = signal<string>('');
  busDetails = signal<BusModel | null>(null);
  seatList = signal<string[]>([]);
  selectedSeats = signal<string[]>([]);
  totalAmount = signal<number>(0);
  isInitialRequest = signal<boolean>(true);
  isSeatAvailable = signal<boolean>(true);

  @ViewChild('toast') toast!: Toast;
  toastType = signal<ToastType>('info');
  delay = signal<number>(2000);
  constructor() {
    this.activatedRoute.params.subscribe((res: any) => {
      const userId = this.authSrc.userId();
      if (userId) {
        this.userId.set(userId);
      }
      this.busId.set(res.id);
      this.getBusDetailsById();
    })
  }
  onSeatClick(seatNo: string) {
    const selectedSeats = this.selectedSeats();
    if (selectedSeats.includes(seatNo)) {
      const index = selectedSeats.indexOf(seatNo);
      selectedSeats.splice(index, 1);
    }
    else {
      selectedSeats.push(seatNo);
    }
    this.selectedSeats.set(selectedSeats);
    const amount = (this.busDetails()?.price ?? 0) * selectedSeats.length;
    this.totalAmount.set(amount);
  }

  getBusDetailsById() {
    this.masterSrc.getBusDetailsById(this.busId()).subscribe({
      next: (res: IApiResponse) => {
        if (res.statusCode.toString() === "200") {
          if (this.isInitialRequest()) {
            this.toastType.set('success');
            this.toast.show(res.statusMessage);
          }
          this.busDetails.set(res.data);
          const capacity = res.data.totalCapacity;
          const numberArray = Array.from({ length: capacity }, (_, i) => i + 1);
          const stringArray = numberArray.map(num => num.toString());
          this.seatList.set(stringArray);
          this.isSeatAvailable.set(capacity !== res.data.seatNos.length);
        }
        else {
          this.toastType.set('danger');
          this.toast.show(res.statusMessage);
        }
        this.isInitialRequest.set(false);
      },
      error: (error: IApiResponse) => {
        this.toastType.set('danger');
        this.toast.show(error.statusMessage);
      }
    })
  }

  createBooking() {
    const bookingObj = new Booking();
    bookingObj.userId = this.userId() ?? '';
    bookingObj.busId = this.busId();
    bookingObj.fromLocationId = this.busDetails()?.fromLocationId ?? '';
    bookingObj.toLocationId = this.busDetails()?.toLocationId ?? '';
    bookingObj.seatNos = this.selectedSeats();
    bookingObj.totalPrice = this.totalAmount();
    this.userService.createBooking(bookingObj).subscribe({
      next: (res: IApiResponse) => {
        if (res.statusCode.toString() === "200") {
          this.toastType.set('success');
          this.toast.show(res.statusMessage);
          this.selectedSeats.set([]);
          this.getBusDetailsById();
        }
        else {
          this.toastType.set('danger');
          this.toast.show(res.statusMessage);
        }
      },
      error: (error: IApiResponse) => {
        this.toastType.set('danger');
        this.toast.show(error.statusMessage);
      }
    })
  }
}
