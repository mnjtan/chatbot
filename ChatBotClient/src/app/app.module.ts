import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms'; // <-- NgModel lives here



import { AppComponent } from './app.component';
import { LoginComponent } from '../login/login.component';
import { LoginModule } from "../login/login.module";
import { DataService } from '../services/data.service';
import { HttpClientModule } from '@angular/common/http';
import { BotComponent } from '../bot/bot.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    BotComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule
  ],
  providers: [
    DataService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
