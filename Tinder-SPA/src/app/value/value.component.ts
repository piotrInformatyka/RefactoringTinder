import { Component, OnInit } from '@angular/core';
import {HttpClient} from '@angular/common/http';

@Component({
  selector: 'app-value',
  templateUrl: './value.component.html',
  styleUrls: ['./value.component.css']
})
export class ValueComponent implements OnInit {

  constructor(private http: HttpClient) { }
  values: any;

  ngOnInit(): void {
    this.getValues()
  }
  getValues(){
    this.http.get('https://localhost:5001/values').subscribe(response => {
      this.values = response;
    }, error => { console.log(error); }
    );
  }


}
