import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, from } from 'rxjs';
import { User } from '../_models/user';
import { map, tap } from 'rxjs/operators';
import { PaginatedResult } from '../_models/pagination';


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

list(page?, itemsPerPage?, userParams?, likesParams?): Observable<PaginatedResult<User[]>> {
  const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<User[]>();

  let params = new HttpParams();

  if(page != null && itemsPerPage != null){
    params = params.append('pageNumber', page);
    params = params.append('pageSize', itemsPerPage);
  }

  if(userParams){
    params = params.append('minAge', userParams.minAge);
    params = params.append('maxAge', userParams.maxAge);
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);
  }
  
  if (likesParams === 'Likers'){
    params = params.append('likers', 'true');
  }
  if (likesParams === 'Likees'){
    params = params.append('likees', 'true');
  }

  return this.http.get<User[]>(this.baseUrl,{ observe: 'response', params})
    .pipe(
      map(response => {
      paginatedResult.result = response.body;

      if ( response.headers.get('Pagination') != null) {
        paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
      }
      return paginatedResult;
    })
    );
  //sending token manually
  // return this.http.get<User[]>(this.baseUrl, httpOptions);
}

get(id:number): Observable<User> {
  console.log('Fetching user: '+ id);
  return this.http.get<User>(this.baseUrl + '/' + id).pipe(tap( data => {
  console.log(data);
  return data;
  })
  );
  // return this.http.get<User>(this.baseUrl + 'users/' + id, httpOptions);
}

update(id: number, user: User){
  return this.http.put(this.baseUrl + '/' + id, user);
}

sendLike(id: number, recipientId: number) {
  //observe property: changes the type of response that http modules will give back to us
  return this.http.post(this.baseUrl + '/' + id + '/like/' + recipientId, {},{observe: 'response'} );
}

}
