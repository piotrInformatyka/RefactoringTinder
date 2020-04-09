import { Component, OnInit, Input, Output } from '@angular/core';
import { EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  @Output() cancelRegister = new EventEmitter<boolean>();
  model: any = {};
  constructor(private authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit(): void {

  }

  register(){
    this.authService.register(this.model).subscribe(() => {
      this.alertify.success('rejestracja udana');
    }, error => {
       this.alertify.error('wysąpił błąd w rejestracji');

     });
  }
  cancel(){
    this.cancelRegister.emit(false);
  }

}
