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

list(page?, itemsPerPage?, userParams?): Observable<PaginatedResult<User[]>> {
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

}
