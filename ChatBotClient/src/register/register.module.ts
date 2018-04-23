import { NgModule } from "@angular/core";
import { FormsModule } from '@angular/forms'; // <-- NgModel lives here
import { RegisterComponent } from "./register.component";
import { BrowserModule } from '@angular/platform-browser';

@NgModule({
    declarations: [
        RegisterComponent
    ],
    imports: [
        BrowserModule,
        FormsModule
    ],
    exports: [
        RegisterComponent
    ]
})
export class RegisterModule{

}