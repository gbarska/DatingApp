import { Component, OnInit } from '@angular/core';
import { Photo } from 'src/app/_models/photo';
import { UserService } from 'src/app/_services/user.service';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-photo-management',
  templateUrl: './photo-management.component.html',
  styleUrls: ['./photo-management.component.css']
})
export class PhotoManagementComponent implements OnInit {
  users: User[];
  photos: any[]

  constructor(private userService: UserService) { }

  ngOnInit() {
    this.photos = [];
    this.userService.getPhotosForApproval().subscribe((res: User[])=>{
      this.users = res;
      this.fillPhotosArray();
    }, error =>{
      console.log('error while fetching photos');
    })

    this.fillPhotosArray();

  }

  approvePhoto(userId: number, photoId: number){
    this.userService.approvePhoto(userId,photoId).subscribe(() =>{
      console.log('success');
      this.users[photoId].photos.filter(x => x.id !== photoId);
    },error  =>{
      console.log(error);
    });
    // console.log(userId, photoId);
  }

  fillPhotosArray(){
    console.log(this.users);
    if (this.users){
      for (let i=0; i< this.users.length; i++){
        if (this.users[i].photos){
           for (let j =0; j< this.users[i].photos.length; j++){
            this.photos.push({userId: this.users[i].id, userName: this.users[i].knownAs,
               url: this.users[i].photos[j].url, photoId: this.users[i].photos[j].id });
          } 
        }
       
      }
    }
   
  }
  deletePhoto(userId: number, photoId: number){
    this.userService.deletePhoto(userId,photoId).subscribe(() =>{
      console.log('success');
      this.users[photoId].photos.filter(x => x.id !== photoId);
    },error  =>{
      console.log(error);
    });
    // console.log(userId, photoId);
  }
}
