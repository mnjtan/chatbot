import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms'; // <-- NgModel lives here



import { AppComponent } from './app.component';
import { LoginComponent } from '../login/login.component';
import { LoginModule } from "../login/login.module";
import { DataService } from '../services/data.service';
import { HttpClientModule } from '@angular/common/http';
import { BotComponent } from '../bot/bot.component';
import { BotModule } from "../bot/bot.module";
import { RegisterComponent } from '../register/register.component';
import { RegisterModule } from "../register/register.module";

@NgModule({
  declarations: [
    AppComponent
    //LoginComponent,
    //RegisterComponent
    //BotComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    LoginModule,
    BotModule,
    RegisterModule
  ],
  providers: [
    DataService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
