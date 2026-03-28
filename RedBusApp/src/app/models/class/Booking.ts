export class Booking{
    id: string;
    userId: string;
    busId: string;
    fromLocationId: string;
    toLocationId: string;
    seatNos: string[];
    totalPrice: number;
    constructor(){
        this.id = "";
        this.userId = "";
        this.busId = "";
        this.fromLocationId = "";
        this.toLocationId = "";
        this.seatNos = [];
        this.totalPrice = 0;
    }
}