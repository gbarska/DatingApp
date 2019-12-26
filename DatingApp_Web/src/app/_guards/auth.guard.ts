import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService,private router: Router,
     private alertify: AlertifyService){}

  canActivate(next: ActivatedRouteSnapshot): boolean {
    if(this.authService.loggedIn()){
      const roles = next.firstChild.data['roles'] as Array<string>;
      if(roles){
        const match = this.authService.roleMatch(roles);

        if (match) {
          return true;
        }
        else {
          this.router.navigate(['members']);
          this.alertify.error('You are not Authorized to access this area');
        }
      }
      return true;
    }

    this.alertify.error('You shall not pass!');
    this.router.navigate(['/home']);

  }

}
