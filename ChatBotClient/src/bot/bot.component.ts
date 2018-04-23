import { Component, Input } from '@angular/core';

@Component({
  selector: 'bot',
  templateUrl: './bot.component.html',
  styleUrls: ['./bot.component.css']
})
export class BotComponent {
  @Input() name:string;

  title = 'app';

}
