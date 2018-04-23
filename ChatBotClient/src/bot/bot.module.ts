import { NgModule } from "@angular/core";
import { FormsModule } from '@angular/forms'; // <-- NgModel lives here
import { BotComponent } from "./bot.component";
import { BrowserModule } from '@angular/platform-browser';

@NgModule({
    declarations: [
        BotComponent
    ],
    imports: [
        BrowserModule,
        FormsModule
    ],
    exports: [
        BotComponent
    ]
})
export class BotModule{

}