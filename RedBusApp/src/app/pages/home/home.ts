import { Component, inject, OnInit, signal, ViewChild } from '@angular/core';
import { Location as RedBusLocation } from '../../models/class/Location'
import { MasterService } from '../../services/master-service';
import { Toast } from '../../shared/reusableComponents/toast/toast';
import { ToastType } from '../../models/toast.model.ts';
import { IApiResponse } from '../../models/interface/ApiResponse';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ITrendingRoute } from '../../models/interface/TrendingRoute';

@Component({
  selector: 'app-home',
  imports: [Toast, FormsModule],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {
  locations = signal<RedBusLocation[]>([]);
  locationObj: RedBusLocation = new RedBusLocation();
  trendingRoutes = signal<ITrendingRoute[]>([]);

  masterSrc = inject(MasterService);
  router = inject(Router);

  fromLocation = signal<string>('');
  toLocation = signal<string>('');
  @ViewChild('toast') toast!: Toast;
  toastType = signal<ToastType>('info');
  delay = signal<number>(2000);

  ngOnInit(): void {
    this.getLocations();
    this.getTrendingRoutes();
  }
  getLocations() {
    this.masterSrc.getAllLocations().subscribe({
      next: (res: IApiResponse) => {
        if (res.statusCode.toString() === "200") {
          this.locations.set(res.data);
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

  getTrendingRoutes() {
    this.masterSrc.trendingRoutes().subscribe({
      next: (res: IApiResponse) => {
        if (res.statusCode.toString() === "200") {
          this.trendingRoutes.set(res.data);
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

  searchBus() {
    const fromLocation = this.fromLocation();
    const toLocation = this.toLocation();
    const url = `search-bus/${fromLocation}/${toLocation}`
    this.router.navigateByUrl(url);
  }
}
