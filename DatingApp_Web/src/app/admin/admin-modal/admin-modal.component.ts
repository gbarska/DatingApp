import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { AdminService } from 'src/app/_services/admin.service';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-admin-modal',
  templateUrl: './admin-modal.component.html',
  styleUrls: ['./admin-modal.component.css']
})
export class AdminModalComponent implements OnInit {
  user: User;
  roles: any[];
 @Output() updateSelectedRoles = new EventEmitter();

  constructor(public bsModalRef: BsModalRef) {}
  ngOnInit() {
  }


  updateRoles(){
    this.updateSelectedRoles.emit(this.roles);
    this.bsModalRef.hide();
  }
}
