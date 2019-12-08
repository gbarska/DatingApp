import { Component, OnInit, ViewChild } from '@angular/core';
import { User } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery';
import { TabsetComponent, BsModalRef, BsModalService } from 'ngx-bootstrap';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  user: User;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  modalRef: BsModalRef;
  @ViewChild('memberTabs') memberTabs: TabsetComponent;
  @ViewChild('template') template;

  constructor(private userService: UserService, private alertify: AlertifyService,
    private modalService: BsModalService, private route: ActivatedRoute, public authService: AuthService) { }

  ngOnInit() {
    // this.loadUser();
    this.route.data.subscribe(data => {
      this.user = data['user'];
    });

    this.route.queryParams.subscribe( params => {
      const selectTab = params['tab'];
      this.memberTabs.tabs[selectTab > 0 ? selectTab : 0].active = true;
    })

    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }
    ];

    this.galleryImages = this.getImages();
  }

  getImages(){
    const imageUrls = [];

    for (const photo of this.user.photos) {
      imageUrls.push({
        small: photo.url,
        medium: photo.url,
        big: photo.url,
        description: photo.description

      });
    }
    return imageUrls;
  }

  selectTab(tabId: number){
    this.memberTabs.tabs[tabId].active = true;
  }
  // loadUser(){
  //   this.userService.get(+this.route.snapshot.params['id']).subscribe((user: User) =>{
  //     this.user = user;
  //     console.log(user);
  //   }, error=> {
  //     this.alertify.error(error);
  //   });
  // }


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
}
