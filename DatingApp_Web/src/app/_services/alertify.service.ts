import { Injectable } from '@angular/core';
import * as alertify from 'alertifyjs';

@Injectable({
  providedIn: 'root'
})
export class AlertifyService {

constructor() { }

confirm(message: string, okCallback: ()=> any){
  alertify.confirm(message, (e:any) => {
    if(e){
      okCallback();
    }else{}
  });
}

confirmWithTitle(title: string, message: string, okCallback: () => any) {
  alertify.confirm(title, message, ( e: any) => {
    if ( e ) {
      console.log(e);
      okCallback();
    }
    // else{
    // }
  }, (f : any ) => {}
  );
}

success(message: string){
  alertify.success(message);
}

error(message: string){
  alertify.error(message);
}

warning(message: string){
  alertify.warning(message);
}

message(message: string){
  alertify.message(message);
}


}
