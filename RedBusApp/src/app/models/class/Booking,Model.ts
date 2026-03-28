import { Booking } from "./Booking";

export class BookingModel extends Booking{
    busName: string;
    fromLocationName: string;
    toLocationName: string;
    constructor(){
        super()
        this.busName = '';
        this.fromLocationName = '';
        this.toLocationName = '';
    }
}