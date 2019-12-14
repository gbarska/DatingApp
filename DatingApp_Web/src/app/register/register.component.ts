import { Component, OnInit, Output, Input, EventEmitter, ViewChild } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { User } from '../_models/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: User;
  bsConfig: Partial<BsDatepickerConfig>;
  // @Input() valuesFromHome: any;
  @Output() cancelRegister = new EventEmitter();
  @ViewChild('confirmPassword', {static: true}) input;
  registerForm: FormGroup;

  constructor(private authService: AuthService, private alertify: AlertifyService, private fb: FormBuilder, private router: Router) { }

  ngOnInit() {
    // this.registerForm =  new FormGroup({
    //   username: new FormControl('', Validators.required),
    //   password: new FormControl('',
    //   [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
    //   confirmPassword: new FormControl('', Validators.required)
    // }, this.passwordMatchValidator);
    this.bsConfig = {
      containerClass: 'theme-default'
    };
    this.createRegisterForm();
  }

passwordMatchValidator(g: FormGroup){
  return g.get('password').value === g.get('confirmPassword').value ? null : {'mismatch': true};
}

  register() {
    if(this.registerForm.valid) {
      this.model = Object.assign({}, this.registerForm.value);

      this.authService.register(this.model).subscribe(() => {
            this.alertify.success('Registration successful');
            this.cancelRegister.emit(false);
          }, error => {
            this.alertify.error(error);
          }, () => {
            this.authService.login(this.model).subscribe(() =>{
              this.router.navigate(['/members']); 
            });
          });
   }
}


createRegisterForm(){
  this.registerForm = this.fb.group({
    gender: ['male'],
    knownAs: ['', Validators.required],
    dateOfBirth: [null, Validators.required],
    city: ['', Validators.required],
    country: ['', Validators.required],
    username: ['', Validators.required],
    password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
    confirmPassword: ['', Validators.required]
  }, {validator: this.passwordMatchValidator});
}


  cancel() {
    this.cancelRegister.emit(false);
  }

}
