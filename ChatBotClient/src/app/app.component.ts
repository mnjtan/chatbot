import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  state: string;
  loggedIn:boolean;
  userName: string;

  title = 'app';

  constructor(){
    this.state = "login";
    this.loggedIn = false;
    this.userName = "Guest";
  }

  onNotify(message: any ):void {
    if(typeof message === 'string'){
      this.state = message;
      var x = document.getElementById('botwindow');
      var inner = document.getElementById("innerDiv");
      if(message === 'bot') {
        x.style.visibility = 'visible';
        inner.className = "col-md-offset-3 col-md-7"; 
      }
      else
      {
        x.style.visibility = 'hidden';
        inner.className = "col-md-offset-3 col-md-1"; 
      }
    }
    else if(typeof message === 'boolean'){
      this.loggedIn = message;
    }
    console.log(message);
  }

  onNotifyName(message: string):void {
    this.userName = message;
    console.log(message);
  }
  

  goSignUp()
  {
    this.state = "register"
    var x = document.getElementById('botwindow');
    x.style.visibility = 'hidden';
    document.getElementById("innerDiv").className = "col-md-offset-3 col-md-1";
  }

  goLogin(){
    this.state = "login"
    var x = document.getElementById('botwindow');
    x.style.visibility = 'hidden';
    document.getElementById("innerDiv").className = "col-md-offset-3 col-md-1";
  }

  goSignOut(){
    this.state = "login"
    this.loggedIn = false;
    this.userName =  "Guest";
    var x = document.getElementById('botwindow');
    x.style.visibility = 'hidden';
    document.getElementById("innerDiv").className = "col-md-offset-3 col-md-1";
  }

}
