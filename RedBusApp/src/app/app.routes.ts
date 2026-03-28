import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { Login } from './pages/login/login';
import { Register } from './pages/register/register';
import { SearchBus } from './pages/search-bus/search-bus';
import { BusBooking } from './pages/bus-booking/bus-booking';
import { BookingList } from './pages/booking-list/booking-list';
import { AdminDashboard } from './pages/admin-dashboard/admin-dashboard';
import { NotFound } from './pages/not-found/not-found';
import { authGuard } from './shared/guards/auth-guard';

export const routes: Routes = [
    {
        path: "",
        redirectTo: "home",
        pathMatch: "full"
    },
    {
        path: "home",
        component: Home
    },
    {
        path: "login",
        component: Login
    },
    {
        path: "register",
        component: Register
    },
    {
        path: "search-bus/:fromLocation/:toLocation",
        component: SearchBus
    },
    {
        path: "bus-booking/:id",
        component: BusBooking
    },
    {
        path: "my-bookings",
        component: BookingList
    },
    {
        path: "admin-dashboard",
        component: AdminDashboard,
        canActivate: [authGuard]
    },
    { path: "**", component: NotFound },
];
