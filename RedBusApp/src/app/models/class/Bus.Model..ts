import { Bus } from "./Bus";

export class BusModel extends Bus {
    fromLocationName: string;
    toLocationName: string;
    seatNos: string[];

    constructor() {
        super(); // calls Bus constructor
        // any extra initialization goes here
        this.fromLocationName = "";
        this.toLocationName = "";
        this.seatNos = [];
    }
}
