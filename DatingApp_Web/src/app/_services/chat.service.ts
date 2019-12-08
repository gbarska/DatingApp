import { Injectable } from '@angular/core';
import { Subject,BehaviorSubject } from 'rxjs';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  isDisplaying = new Subject<boolean>();
  recipient = new BehaviorSubject<User>(null);

  show() {
      this.isDisplaying.next(true);
  }
  hide() {
      this.isDisplaying.next(false);
  }

  setRecipientId(user: User){
      this.recipient.next(user);
  }

}