import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;

  constructor(private http: HttpClient) { }
    values: any;
  ngOnInit() {
    // this.getValues();
  }

  registerTogle(){
    this.registerMode = true;
  }


  // getValues() {
  //   this.http.get('http://localhost:5000/api/Main').subscribe( response=> {
  //     this.values = response;
  //   }, error => {
  //     console.log(error);
  //   })
  // }

  cancelRegisterMode(event: boolean) {
    this.registerMode = event;
  }

}
