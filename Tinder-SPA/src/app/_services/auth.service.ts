import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {map} from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import {BehaviorSubject} from 'rxjs';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  jwtHelper = new JwtHelperService();
  baseUrl = environment.apiUrl + 'auth/';
  decodedToken: any;
  currentUser: User;
  token: any;
  photoUrl = new BehaviorSubject<string>('../../assets/360px-Yu-Hwan.png');
  currentPhotoUrl = this.photoUrl.asObservable();

constructor(private http: HttpClient) { }

changeUserPhoto(photoUrlArg : string){
  this.photoUrl.next(photoUrlArg);
}

login(model: any){
  return this.http.post(this.baseUrl + 'login', model)
    .pipe(map((response: any) => {
      const user = response;
      if (user){
        localStorage.setItem('token', user.token);
        localStorage.setItem('user', JSON.stringify(user.user));
        this.decodedToken = this.jwtHelper.decodeToken(user.token);
        this.currentUser = user.user;
        this.token = user.token;
        this.changeUserPhoto(this.currentUser.photoUrl);
        console.log(this.decodedToken);
        console.log(this.token);
        console.log(this.currentUser);
      }
    }));
}
  register(user: User)
  {
    return this.http.post(this.baseUrl + 'register', user);
  }
  loggedIn(){
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }
}
