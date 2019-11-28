import { Injectable } from '@angular/core';
import { User } from '../_models/user';
import { Resolve, Router, ActivatedRoute } from '@angular/router';
import {ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { UserService } from '../_services/user.service';
import { AuthService } from '../_services/auth.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';


@Injectable()
export class MemberEditResolver implements Resolve<User>{
  constructor(private userService: UserService, private router: Router,
    private authService: AuthService, private alertify: AlertifyService){}

  resolve(route: ActivatedRouteSnapshot): Observable<User> {
      return this.userService.get(this.authService.decodedToken.nameid).pipe(
          catchError(error => {
              this.alertify.error('Problem retrieving data');
              this.router.navigate(['/members']);
              return of(null);
          })
      );
  }
}