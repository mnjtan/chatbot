import { Component, EventEmitter, Output } from "@angular/core";
import { User } from "../login/user";
import { DataService } from "../services/data.service";


@Component({
    selector: 'register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css']
})
export class RegisterComponent{
    @Output() notifyState: EventEmitter<string> = new EventEmitter<string>();
    // @Output() notifyLoggedIn: EventEmitter<boolean> = new EventEmitter<boolean>();

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
            this.Error ="An account already exists for that email!";
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
        //redirect to signIn page
        this.notifyState.emit("login");
        // this.notifyLoggedIn.emit(false);
    }

    //on submit register form
    register(){
        this.Error = null;

        console.log(this.user);
        console.log(JSON.stringify(this.user));
        
        //call the service
        this.dataClient.registerUser(this.user).subscribe(
            data => {
                this.setUser(data);
                console.log("SUCCESS",this.verifiedUser);
                
            },
            error => {
                this.verifiedUser = null;
                this.Error ="An account already exists for that email!";
                console.log("FAILURE",this.verifiedUser);
            }
        );
    }

}