import { Component, EventEmitter, Output } from "@angular/core";
import { User } from "./user";
import { DataService } from "../services/data.service";


@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent{
    @Output() notifyState: EventEmitter<string> = new EventEmitter<string>();
    @Output() notifyLoggedIn: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() notifyName: EventEmitter<string> = new EventEmitter<string>();

    user: User;
    verifiedUser: User;
    dataClient: DataService;
    Error: string;

    constructor(dataClient: DataService){
        this.user = new User();
        this.verifiedUser = new User();
        this.dataClient = dataClient;
        this.Error = null;
    }

    setUser(data: Object){
        if (data == null){
            this.verifiedUser = null;
            this.Error = "We do not recognize your email and/or password. Please try again or Register for an account.";
            return;
        }
        this.verifiedUser = {
            userId: data['userId'],
            email: data['email'],
            name: data['name'],
            phone: data['phone'],
            team: data['team']
        }
        //alert container about sign in, hide signin / show bot
        this.notifyState.emit("bot");
        this.notifyLoggedIn.emit(true);
        this.notifyName.emit(this.verifiedUser.name);
    }

    submit(){
        this.Error = null;

        if(this.user.email == null || this.user.email === ""){
            this.Error = "We do not recognize your email and/or password. Please try again or Register for an account.";
            return;
        }

        //call the service
        this.dataClient.signIn(this.user.email).subscribe(data => {
            this.setUser(data);
            console.log("second",this.verifiedUser);
        });
    }

    guestSubmit(){
        console.log("Continue as guest");
        this.notifyState.emit("bot");
        this.notifyName.emit("Guest");
        
    }

}