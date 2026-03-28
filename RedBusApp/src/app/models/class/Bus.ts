export class Bus{
    id: string;
    name: string;
    fromLocationId: string;
    toLocationId: string;
    departTime: string;
    arriveTime: string;
    totalCapacity: number;
    price: number;
    constructor(){
        this.id = "";
        this.name = "";
        this.fromLocationId = "";
        this.toLocationId = "";
        this.departTime = "";
        this.arriveTime = "";
        this.totalCapacity = 0;
        this.price = 0;
    }
}