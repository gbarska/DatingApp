import { Component, OnInit } from '@angular/core';
import { AdminService } from 'src/app/_services/admin.service';
import { User } from 'src/app/_models/user';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { AdminModalComponent } from '../admin-modal/admin-modal.component';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {
users: User[];
bsModalRef: BsModalRef;

constructor(private modalService: BsModalService,
  private adminService: AdminService) { }

  ngOnInit() {
    this.getUsersWithRoles();
  }
  getUsersWithRoles(){
    this.adminService.getUsersWithRoles().subscribe((users: User[]) => {
        this.users = users;
    },
    error => {
      console.log('error while fetching users with role: '+ error);
    });
    
  }

  editRolesModal(user: User){
    const initialState = {
      user,
      roles: this.getRolesArray(user)
    };

    this.bsModalRef = this.modalService.show(AdminModalComponent, {initialState});
    this.bsModalRef.content.updateSelectedRoles.subscribe((values) => {
      const rolesToUpdate = {
        roleNames: [...values.filter(el => el.checked === true).map(el => el.name)]
      }

      if (rolesToUpdate){
      this.adminService.updateUserRoles(user, rolesToUpdate).subscribe(() => {
        user.roles = [...rolesToUpdate.roleNames];
      }, error => {
        console.log('failed to update roles..');
      });
    }
    });
  }

  private getRolesArray(user):any[] {
    const roles = [];
    const userRoles = user.roles;
    const availableRoles: any[] = [
      {name: 'Admin', value: 'Admin'},
      {name: 'Moderator', value: 'Moderator'},
      {name: 'Member', value: 'Member'},
      {name: 'VIP', value: 'VIP'}
    ];

    for(let i =0; i< availableRoles.length ; i++){
      let isMatch = false;

      for(let j = 0 ; j < userRoles.length; j++){
        if(availableRoles[i].name == userRoles[j]){
          isMatch = true;
          availableRoles[i].checked = true;
          roles.push(availableRoles[i]);
          break;
        }
      }

      if (!isMatch){
        availableRoles[i].checked = false;
        roles.push(availableRoles[i]);
      }
    }
    return roles;
  }
}

