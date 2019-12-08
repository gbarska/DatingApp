import { Component, OnInit } from '@angular/core';
import { ChatService } from '../_services/chat.service';
import { Subscription } from 'rxjs';
import { User } from '../_models/user';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {
  constructor(private chatService: ChatService) { }
  recipient: User;
  subs: Subscription;

  ngOnInit() {
  this.subs = this.chatService.recipient.subscribe(next => {
    this.recipient = next;
  });
  }

   onClose(){
     this.chatService.hide();
   }
ngOnDestroy(){
  this.subs.unsubscribe();
}

}
