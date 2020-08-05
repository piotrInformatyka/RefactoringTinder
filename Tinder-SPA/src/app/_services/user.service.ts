import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, ObservableLike } from 'rxjs';
import { User } from '../_models/user';

const httpOptions = {
  headers: new HttpHeaders({
    'Authorization': 'Bearer ' + localStorage.getItem('token'),
  })
}

@Injectable({
  providedIn: 'root'
})
export class UserService {

  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  getUsers(): Observable<User[]>{
    return this.http.get<User[]>(this.baseUrl + 'users/', httpOptions);
  }
  getUser(id: number): Observable<User>{
    return this.http.get<User>(this.baseUrl + 'users/' + id, httpOptions);
  }
  updateUser(id: number, user: User){
    console.log(httpOptions.headers);
    return this.http.put(this.baseUrl + 'users/' + id, user, httpOptions);
  }
  setMainPhoto(userId: number, id: number){
    return this.http.post(this.baseUrl + 'users/' + userId + '/photos/' + id + '/setMain', {}, httpOptions);
  }
  deletePhoto(userId: number, id: number){
    return this.http.delete(this.baseUrl + 'users/' + userId + '/photos/' + id, httpOptions);
  }
}
