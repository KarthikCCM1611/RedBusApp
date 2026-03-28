import { Component, inject, OnInit, signal, ViewChild } from '@angular/core';
import { MasterService } from '../../services/master-service';
import { Toast } from '../../shared/reusableComponents/toast/toast';
import { ToastType } from '../../models/toast.model.ts';
import { IApiResponse } from '../../models/interface/ApiResponse';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { BusModel } from '../../models/class/Bus.Model.';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-search-bus',
  imports: [Toast, DatePipe, RouterLink],
  templateUrl: './search-bus.html',
  styleUrl: './search-bus.css',
})
export class SearchBus implements OnInit {
  masterSrc = inject(MasterService);
  activatedRoute = inject(ActivatedRoute);

  fromLocationId = signal<string>('');
  toLocationId = signal<string>('');
  busList = signal<BusModel[]>([]);
  errorMsg = signal<string>('');
  isSearching = signal<boolean>(true);

  @ViewChild('toast') toast!: Toast;
  toastType = signal<ToastType>('info');
  delay = signal<number>(2000);
  ngOnInit(): void {
    this.activatedRoute.params.subscribe((res: any) => {
      this.fromLocationId.set(res.fromLocation);
      this.toLocationId.set(res.toLocation);
      this.searchBusByLocation();
    })
  }

  searchBusByLocation() {
    this.masterSrc.searchBuses(this.fromLocationId(), this.toLocationId()).subscribe({
      next: (res: IApiResponse) => {
        this.isSearching.set(false);
        if (res.statusCode.toString() === "200") {
          this.toastType.set('success');
          this.toast.show(res.statusMessage);
          this.busList.set(res.data);
        }
        else {
          this.toastType.set('danger');
          this.toast.show(res.statusMessage);
          this.errorMsg.set(res.statusMessage);
        }
      },
      error: (error: IApiResponse) => {
        this.isSearching.set(false);
        this.toastType.set('danger');
        this.toast.show(error.statusMessage);
      }
    })
  }
}
