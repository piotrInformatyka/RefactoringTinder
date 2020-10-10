import { Component, OnInit, ViewChild } from '@angular/core';
import { User } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import {NgxGalleryOptions} from '@kolkov/ngx-gallery';
import {NgxGalleryImage} from '@kolkov/ngx-gallery';
import {NgxGalleryAnimation} from '@kolkov/ngx-gallery';
import { DatePipe } from '@angular/common';
import { viewClassName } from '@angular/compiler';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent implements OnInit {

  @ViewChild('userTabs', {static: true}) userTabs: TabsetComponent;

  user: User;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  createdAt: Date;
  lastActive: Date;
  constructor(private userService: UserService, private alertify: AlertifyService,
              private route: ActivatedRoute, private datePipe: DatePipe, private authService: AuthService) {  }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data.user;
      this.createdAt = this.user.createdAt;
      this.lastActive = this.user.lastActive;
    });
    this.route.queryParams.subscribe(params => {
      const selectTab = params.tab;
      this.userTabs.tabs[selectTab > 0 ? selectTab : 0].active = true;
    })

    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        thumbnailsColumns: 4,
        imagePercent: 100,
        preview: false,
        imageAnimation: NgxGalleryAnimation.Slide
      },
    ];

    this.galleryImages = this.getImages();
  }

  getImages() {
    const imagesUrls = [];
    // tslint:disable-next-line: prefer-for-of
    for (let i = 0; i < this.user.photos.length; i++) {
      imagesUrls.push({
        small: this.user.photos[i].url,
        medium: this.user.photos[i].url,
        big: this.user.photos[i].url,
        desciption: this.user.photos[i].description
      });
    }
    return imagesUrls;
  }
  selectTab(tabId: number){
    this.userTabs.tabs[tabId].active = true;
  }
  sendLike(recipient: number){
    this.userService.sendLike(this.authService.decodedToken.nameid, recipient)
    .subscribe(data => {
      this.alertify.success('Polubiłeś: ' + this.user.username);
    },
    error => {
      this.alertify.error(error);
    });
  }

}




  // loadUsers(){
  //   this.userService.getUser(+this.route.snapshot.params['id']).subscribe((user: User) => {
  //     this.user = user;
  //   },  error => {this.alertify.error(error); });
  // }


