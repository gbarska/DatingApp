import { Component, OnInit, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Post } from './post.model';
import { Subscription } from 'rxjs';
import { AuthService } from './_services/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  jwtHelper = new JwtHelperService();
  
  ngOnInit() {
    const token = localStorage.getItem('token');
    if(token){
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);
    }
  }
  
constructor(private http: HttpClient, private authService: AuthService) {}


  
}
