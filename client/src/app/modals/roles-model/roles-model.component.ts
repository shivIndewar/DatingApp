import { Component, EventEmitter, Input, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { user } from 'src/app/_models/user';

@Component({
  selector: 'app-roles-model',
  templateUrl: './roles-model.component.html',
  styleUrls: ['./roles-model.component.css']
})
export class RolesModelComponent implements OnInit {

  @Input() updateSelectedRoles= new EventEmitter();
  user:user;
  roles : any[]; 
 
  constructor(public bsModalRef : BsModalRef) { }

  ngOnInit(): void {
  }

  updateRoles(){
    this.updateSelectedRoles.emit(this.roles);
    console.log(this.roles);
    this.bsModalRef.hide();
  }

}