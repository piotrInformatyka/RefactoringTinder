import { Component, OnInit, Input, Output } from '@angular/core';
import { EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { User } from '../_models/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  @Output() cancelRegister = new EventEmitter<boolean>();
  user: User;
  registerForm: FormGroup;
  zodiacSignList = [{value: 'Baran', display: 'Baran'},
                    {value: 'Byk', display: 'Byk'},
                    {value: 'Bliźnięta', display: 'Bliźnięta'},
                    {value: 'Rak', display: 'Rak'},
                    {value: 'Lew', display: 'Lew'},
                    {value: 'Panna', display: 'Panna'},
                    {value: 'Waga', display: 'Waga'},
                    {value: 'Skorpion', display: 'Skorpion'},
                    {value: 'Ryby', display: 'Ryby'}];
  userParams: any = {};
  constructor(private authService: AuthService, private alertify: AlertifyService, private fb: FormBuilder, private router: Router) { }

  ngOnInit(): void {
    this.createRegisterForm();
  }

  createRegisterForm(){
    this.registerForm = this.fb.group({
      username: ['' , Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(10)]],
      confirmPassword: ['', Validators.required],
      gender: ['kobieta'],
      dateOfBirth: [null, Validators.required],
      zodiacSign: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required]
    }, {validator: this.passwordMatchValidator});
  }

  passwordMatchValidator(fg: FormGroup){
    return fg.get('password').value === fg.get('confirmPassword').value ? null : {mismatch: true};
  }

  register(){
    if(this.registerForm.valid){

      this.user = Object.assign({}, this.registerForm.value);

      this.authService.register(this.user).subscribe(() => {
        this.alertify.success('rejestracja udana');
       }, error => {
       this.alertify.error(error);
      }, () => {
        this.authService.login(this.user).subscribe(() => {
          this.router.navigate(['/uzytkownicy']);
        });
      });
    }
  }
  cancel(){
    this.cancelRegister.emit(false);
  }

}
