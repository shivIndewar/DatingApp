import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {map} from 'rxjs/operators';
import { user } from '../_models/user';
import { ReplaySubject } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl ='https://localhost:5001/api/';
  private currenyUserSource = new ReplaySubject<user>(1);
  currentUser$ = this.currenyUserSource.asObservable();

  constructor(private http : HttpClient, private toastr: ToastrService) { }

  login(model:any){
    return this.http.post(this.baseUrl + 'account/login', model).pipe(
      map((response : user) =>{
        const user = response;
         if(response){
          localStorage.setItem('user', JSON.stringify(user));
          this.currenyUserSource.next(user);
         }
         return user;
      })
    )}

    register(model:any){
      return this.http.post(this.baseUrl + 'account/register',model).pipe(
        map((user: user) =>{
          if(user){
            localStorage.setItem('user', JSON.stringify(user));
            this.currenyUserSource.next(user);
          }
        })
      )
    }

    setCurrentUser(user : user){
      this.currenyUserSource.next(user);
    }

    logout(){
      localStorage.removeItem('user');
      this.currenyUserSource.next(null);
    }
  }
