import { Component, OnInit, Input, AfterViewInit } from '@angular/core';
import { Message } from 'src/app/_models/message';
import { ActivatedRoute } from '@angular/router';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { tap } from 'rxjs/operators';
import { ChatService } from 'src/app/_services/chat.service';
import { Subscribable, Subscriber, Subscription, interval } from 'rxjs';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {

  // @Input() recipientId: number;
  messages: Message[];
  newMessage: any = {};
  subs: Subscription;
  recipient: User;
  timeSubs: Subscription;

  constructor(private userService: UserService, private authService: AuthService,
    private route: ActivatedRoute, private alertify: AlertifyService, private chatService: ChatService ) { }

  ngOnInit() {
    this.subs = this.chatService.recipient.subscribe(next => {
      this.recipient = next;
      this.loadMessages();
    });

    this.timeSubs = interval(1500).subscribe( () => {
      this.loadMessages();
    });
    // console.log('recipient id'+ this.recipientId);
  }

loadMessages(){
  const currentUserId = +this.authService.decodedToken.nameid;
  this.userService.getMessageThread(this.authService.decodedToken.nameid, this.recipient.id)
    .pipe(
      tap(messages => {
        for (let i = 0; i < messages.length; i++){
          if (messages[i].isRead === false && messages[i].recipientId === currentUserId){
            this.userService.markAsRead(messages[i].id, currentUserId);
          }
        }
      })
    )
    .subscribe( messages => {
      this.messages = messages;
    }, error =>{
      this.alertify.error(error);
    })
}

sendMessage(){
  this.newMessage.recipientId = this.recipient.id;
  this.userService.sendMessage(this.authService.decodedToken.nameid, this.newMessage).subscribe( (message: Message) =>{
    this.messages.unshift(message);
    this.newMessage.content = '';
  }, error => {
    this.alertify.error(error);
  });
}

ngOnDestroy(): void {
  this.subs.unsubscribe();
  this.timeSubs.unsubscribe();
}

}
