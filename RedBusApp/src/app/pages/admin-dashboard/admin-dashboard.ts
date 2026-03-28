import { Component, inject, OnInit, signal, ViewChild } from '@angular/core';
import { AdminService } from '../../services/admin-service';
import { IApiResponse } from '../../models/interface/ApiResponse';
import { Toast } from '../../shared/reusableComponents/toast/toast';
import { ToastType } from '../../models/toast.model.ts';
import { Bus } from '../../models/class/Bus';
import { Location as RedBusLocation } from '../../models/class/Location'
import { FormsModule, NgForm } from '@angular/forms';
import { MasterService } from '../../services/master-service';
import { DatePipe } from '@angular/common';
import { BusModel } from '../../models/class/Bus.Model.';

@Component({
  selector: 'app-admin-dashboard',
  imports: [Toast, FormsModule, DatePipe],
  templateUrl: './admin-dashboard.html',
  styleUrl: './admin-dashboard.css',
})
export class AdminDashboard implements OnInit {

  locations = signal<RedBusLocation[]>([]);
  buses = signal<BusModel[]>([]);
  locationObj: RedBusLocation = new RedBusLocation();
  busObj: BusModel = new BusModel();

  adminSrc = inject(AdminService);
  masterSrc = inject(MasterService);

  @ViewChild('toast') toast!: Toast;
  @ViewChild('busForm') busForm!: NgForm;
  @ViewChild('locForm') locForm!: NgForm;
  toastType = signal<ToastType>('info');
  delay = signal<number>(2000);
  initialRequest = signal<boolean>(true);
  ngOnInit(): void {
    this.getLocations();
    this.getBuses();
    this.initialRequest.set(false);
  }

  getLocations() {
    this.masterSrc.getAllLocations().subscribe({
      next: (res: IApiResponse) => {
        if (res.statusCode.toString() === "200") {
          if (this.initialRequest()) {
            this.toastType.set('success');
            this.toast.show(res.statusMessage);
          }
          this.locations.set(res.data);
        }
        else {
          if (this.initialRequest()) {
            this.toastType.set('danger');
            this.toast.show(res.statusMessage);
          }
        }
      },
      error: (error: IApiResponse) => {
        this.toastType.set('danger');
        this.toast.show(error.statusMessage);
      }
    })
  }

  addLocation() {
    if (this.locationObj.name === "" || !this.locationObj.name) {
      this.toastType.set('warning');
      this.toast.show("Provide the proper location name");
      return;
    }
    this.adminSrc.addLocation(this.locationObj).subscribe({
      next: (res: IApiResponse) => {
        if (res.statusCode.toString() === "200") {
          this.toastType.set('success');
          this.toast.show(res.statusMessage);
          this.getLocations();
          this.clearLocation();
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

  updateLocation() {
    this.adminSrc.updateLocation(this.locationObj).subscribe({
      next: (res: IApiResponse) => {
        if (res.statusCode.toString() === "200") {
          this.toastType.set('success');
          this.toast.show(res.statusMessage);
          this.getLocations();
          this.clearLocation();
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

  editLocation(location: RedBusLocation) {
    this.locationObj = structuredClone(location);
  }

  deleteLocation(location: RedBusLocation) {
    const result = confirm("Do you want to delete the location?")
    if (!result) {
      return;
    }
    this.adminSrc.deleteLocation(location.id).subscribe({
      next: (res: IApiResponse) => {
        if (res.statusCode.toString() === "200") {
          this.toastType.set('success');
          this.toast.show(res.statusMessage);
          this.getLocations();
          this.clearLocation();
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

  clearLocation() {
    this.locForm.reset();
    this.locationObj = new RedBusLocation();
  }

  clearBus() {
    this.busForm.resetForm({ fromLocation: '', toLocation: '', price: 0, totalCapacity: 0 });
    this.busObj = new BusModel();
  }

  getBuses() {
    this.masterSrc.getAllBuses().subscribe({
      next: (res: IApiResponse) => {
        if (res.statusCode.toString() === "200") {
          if (this.initialRequest()) {
            this.toastType.set('success');
            this.toast.show(res.statusMessage);
          }
          this.buses.set(res.data);
        }
        else {
          if (this.initialRequest()) {
            this.toastType.set('danger');
            setTimeout(() => {
              this.toast.show(res.statusMessage);
            }, 3000)
          }
        }
        if (this.initialRequest()) {
          this.initialRequest.set(false);
        }
      },
      error: (error: IApiResponse) => {
        this.toastType.set('danger');
        this.toast.show(error.statusMessage);
      }
    })
  }

  addBus() {
    this.adminSrc.addBus(this.busObj).subscribe({
      next: (res: IApiResponse) => {
        if (res.statusCode.toString() === "200") {
          this.toastType.set('success');
          this.toast.show(res.statusMessage);
          this.getBuses();
          this.clearBus();
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

  editBus(bus: BusModel) {
    this.busObj = structuredClone(bus);
  }

  updateBus() {
    this.adminSrc.updateBus(this.busObj).subscribe({
      next: (res: IApiResponse) => {
        if (res.statusCode.toString() === "200") {
          this.toastType.set('success');
          this.toast.show(res.statusMessage);
          this.getBuses();
          this.clearBus();
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

  deleteBus(bus: Bus) {
    const result = confirm("Do you want to delete the bus?")
    if (!result) {
      return;
    }
    this.adminSrc.deleteBus(bus.id).subscribe({
      next: (res: IApiResponse) => {
        if (res.statusCode.toString() === "200") {
          this.toastType.set('success');
          this.toast.show(res.statusMessage);
          this.getBuses();
          this.clearBus();
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
