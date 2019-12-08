import { Component, OnInit, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Post } from './post.model';
import { Subscription } from 'rxjs';
import { AuthService } from './_services/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { User } from './_models/user';
import { ChatService } from './_services/chat.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  jwtHelper = new JwtHelperService();
  isChatOpen = false;
  subs: Subscription;

  ngOnInit() {
    const token = localStorage.getItem('token');
    const user: User = JSON.parse(localStorage.getItem('user'));

    if(token){
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);
    }
    if(user){
      this.authService.currentUser = user;
      this.authService.changeMemberPhoto(user.photoUrl);
    }

this.subs = this.chatService.isDisplaying.subscribe(next => {
  this.isChatOpen = next;
});

}
  
constructor(private http: HttpClient, private authService: AuthService, private chatService: ChatService) {}

ngOnDestroy(): void {
  this.subs.unsubscribe();
 }
  
}
