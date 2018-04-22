import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  state: string;
  loggedIn:boolean;

  title = 'app';

  constructor(){
    this.state = "login";
    this.loggedIn = false;
  }

  onNotify(message: any ):void {
    if(typeof message === 'string'){
      this.state = message;
      var x = document.getElementById('botwindow');
      if(message === 'bot') {
        x.style.visibility = 'visible';
      }
      else
      {
        x.style.visibility = 'hidden';
      }
    }
    else if(typeof message === 'boolean'){
      this.loggedIn = message;
    }
    console.log(message);
  }

  goSignUp()
  {
    this.state = "register"
  }

  goLogin(){
    this.state = "login"
  }

  goSignOut(){
    this.state = "login"
    this.loggedIn = false;
  }

}
