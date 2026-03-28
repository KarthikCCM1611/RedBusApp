export class User{
    id: string
    name: string;
    email: string;
    password: string;
    confirmPassword: string;
    phoneNo: string;
    city: string;
    constructor(){
        this.id = "";
        this.name = "";
        this.email = "";
        this.password = "";
        this.confirmPassword = "";
        this.phoneNo = "";
        this.city = "";
    }
}