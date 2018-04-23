import { NgModule } from "@angular/core";
import { FormsModule } from '@angular/forms'; // <-- NgModel lives here
import { LoginComponent } from "./login.component";
import { BrowserModule } from '@angular/platform-browser';

@NgModule({
    declarations: [
        LoginComponent
    ],
    imports: [
        BrowserModule,
        FormsModule
    ],
    exports: [
        LoginComponent
    ]
})
export class LoginModule{

}