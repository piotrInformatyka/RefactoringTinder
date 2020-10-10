import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, ObservableLike } from 'rxjs';
import { User } from '../_models/user';
import { PaginationResult } from '../_models/pagination';
import { map } from 'rxjs/operators';
import { Message } from '../_models/message';
import { AuthService } from './auth.service';



@Injectable({
  providedIn: 'root'
})
export class UserService {

  baseUrl = environment.apiUrl;
  // tslint:disable-next-line: align
   httpOptions = {
     headers: new HttpHeaders({
      'Authorization': 'Bearer ' + localStorage.getItem('token')
    })
   };
  constructor(private http: HttpClient, private authService: AuthService) {}

  getUsers(page?, itemsPerPage?, userParams?, likesParams?): Observable<PaginationResult<User[]>>{
    const paginationResult: PaginationResult<User[]> = new PaginationResult<User[]>();
    let params = new HttpParams();

    if (page != null && itemsPerPage != null)
    {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }
    if (userParams != null)
    {
      params = params.append('minAge', userParams.minAge);
      params = params.append('maxAge', userParams.maxAge);
      params = params.append('gender', userParams.gender);
      params = params.append('zodiacSign', userParams.zodiacSign);
      params = params.append('orderBy', userParams.orderBy);
    }
    if (likesParams === 'UserLikes')
      {
        params = params.append('UserLikes', 'true');
      }
    if (likesParams === 'UserIsLiked')
      {
        params = params.append('UserIsLiked', 'true');
      }

    return this.http.get<User[]>(this.baseUrl + 'users/', {headers: this.httpOptions.headers, observe: 'response', params})
    .pipe(
      map (response => {
        paginationResult.result = response.body;
        if (response.headers.get('Pagination') != null){
            paginationResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginationResult;
      })
    );
  }
  getUser(id: number): Observable<any>{
    return this.http.get<User>(this.baseUrl + 'users/' + id, this.httpOptions);
  }
  updateUser(id: number, user: User){
    console.log(this.httpOptions.headers);
    return this.http.put(this.baseUrl + 'users/' + id, user, this.httpOptions);
  }
  setMainPhoto(userId: number, id: number){
    return this.http.post(this.baseUrl + 'users/' + userId + '/photos/' + id + '/setMain', {}, this.httpOptions);
  }
  deletePhoto(userId: number, id: number){
    return this.http.delete(this.baseUrl + 'users/' + userId + '/photos/' + id, this.httpOptions);
  }
  sendLike(id: number, recipientId: number){
    return this.http.post(this.baseUrl + 'users/' + id + '/like/' + recipientId, {}, this.httpOptions);
  }
  getMessages(id: number, page?, itemsPerPage?, messageContainer?)
  {
    const paginationResult: PaginationResult<Message[]> = new PaginationResult<Message[]>();
    let params = new HttpParams();

    params = params.append('MessageContainer', messageContainer);
    if(page != null && itemsPerPage != null)
    {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }
    console.log(this.httpOptions);
    // tslint:disable-next-line: max-line-length
    return this.http.get<Message[]>(this.baseUrl + 'users/' + id + '/messages', {headers: this.httpOptions.headers, observe: 'response', params})
    .pipe(
      map (response => {
        paginationResult.result = response.body;
        if (response.headers.get('Pagination') != null){
            paginationResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginationResult;
      })
    );
  }
  getMessageThread(id: number, recipientId: number){
    return this.http.get<Message[]>(this.baseUrl + 'users/' + id + '/messages/thread/' + recipientId, this.httpOptions);
  }
  sendMessage(id: number, message: Message){
    return this.http.post(this.baseUrl + 'users/' + id + '/messages', message, this.httpOptions);
  }
  deleteMessage(id: number, userId: number){
    return this.http.delete(this.baseUrl + 'users/' + userId + '/messages/' + id, this.httpOptions);
  }
  markAsRead(userId: number, messageId: number){
    return this.http.post(this.baseUrl + 'users/' + userId + '/messages/' + messageId + '/read', {}, this.httpOptions).subscribe();
  }

}
