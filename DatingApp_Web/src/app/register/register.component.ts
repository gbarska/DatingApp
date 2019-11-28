import { Component, OnInit, Output, Input, EventEmitter, ViewChild } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: any =  {};
  // @Input() valuesFromHome: any;
  @Output() cancelRegister = new EventEmitter();
  @ViewChild('confirmPassword') input; 

  constructor(private authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
  }

  register() {
    if (this.input.nativeElement.value == this.model.password){
      this.authService.register(this.model).subscribe(() => {
        this.alertify.success("Registration successful");
        this.cancelRegister.emit(false);
      }, error => {
        this.alertify.error(error);
      });
    }else{
      this.alertify.error("passwords don't match");
    }
    // console.log(this.input);
    // console.log(this.model.password);
  }

  cancel() {
    this.cancelRegister.emit(false);
  }

}
