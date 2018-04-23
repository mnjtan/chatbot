import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { User } from "../login/user";

@Injectable()
export class DataService{
    client: HttpClient;
    response: any;

    constructor(client: HttpClient){
        this.client = client;
    }

    signIn(email: string){
        return this.client.get("http://13.59.35.94/chatbotdata/api/data/"+ email);
    }
    
    registerUser(user: User){
        // this.client.get("").toPromise().then(this.pass,this.fail);
        return this.client.post("http://13.59.35.94/chatbotdata/api/data",user);
    }

    pass(response: Object){
        console.log(response);
    }

    fail(response: Object){
        console.log(response);
    }
}