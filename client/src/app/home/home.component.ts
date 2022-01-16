import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
registerMmode =false;
  constructor() { }
  ngOnInit(): void {
  }
  registerToggle(){
    this.registerMmode = !this.registerMmode;
  }

  cancelRegisterMode(event : boolean){
    this.registerMmode = event;
  }

}
