import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, from } from 'rxjs';
import { User } from '../_models/user';
import { map } from 'rxjs/operators';


// const httpOptions = {
//   headers: new HttpHeaders({
//     'Authorization': 'Bearer ' + localStorage.getItem('token')
//   })
// }

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl + '/users';

  constructor(private http: HttpClient) { }

list(): Observable<User[]> {
  return this.http.get<User[]>(this.baseUrl);
  //sending token manually
  // return this.http.get<User[]>(this.baseUrl, httpOptions);
}

get(id:number): Observable<User> {
  console.log('Fetching user: '+id);
  return this.http.get<User>(this.baseUrl + '/' + id).pipe(map( data => {
  console.log(data);
  return data;
  })
  );
  // return this.http.get<User>(this.baseUrl + 'users/' + id, httpOptions);
}

update(id: number, user: User){
  return this.http.put(this.baseUrl + '/' + id, user);
}

}
