import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { user } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AdminService { 

  baseUrl = environment.apiUrl;

  constructor(private http:HttpClient) { }

  getUsersWithRoles(){
    return this.http.get<Partial<user[]>>(this.baseUrl+'admin/users-with-roles');
  }

  updateUserRoles(username:string,roles:string[]){
    return this.http.post(this.baseUrl+'admin/edit-roles/'+username+'?roles='+roles,{});
  }
}
