import { Component, OnInit, Input,  TemplateRef, ViewChild } from '@angular/core';
import { User } from 'src/app/_models/user';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ChatService } from 'src/app/_services/chat.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
  @Input() user: User;
  modalRef: BsModalRef;
  @ViewChild('template', {static: true}) template; 


  constructor(public authService: AuthService, 
    private userService: UserService, private alertify: AlertifyService,
    private modalService: BsModalService, private chatService: ChatService) { }

  ngOnInit() {
  }


  sendLike(id: number) {
    this.userService.sendLike(this.authService.decodedToken.nameid, id)
    .subscribe( responseData => {
      console.log(responseData);
     const match = JSON.parse(responseData.headers.get('likers'));
    //  console.log(match);

      if(match.match === true) {
        this.modalRef = this.modalService.show(this.template);
     } else {
      this.alertify.success('You have liked: ' + this.user.knownAs);
     }
    }, error => {
      // console.log('ops passeiaqui3');
      this.alertify.error(error);
    })

  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  openChat(){
    this.chatService.setRecipientId(this.user);
    this.chatService.show();
  }
  onSendMessage()
  {
    this.modalRef.hide();
    this.openChat();
  }
}
