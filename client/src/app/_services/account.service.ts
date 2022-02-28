import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {map} from 'rxjs/operators';
import { user } from '../_models/user';
import { ReplaySubject } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment';
import { PresenceService } from './presence.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl =environment.apiUrl;
  private currenyUserSource = new ReplaySubject<user>(1);
  currentUser$ = this.currenyUserSource.asObservable();

  constructor(private http : HttpClient, private toastr: ToastrService, private presence : PresenceService) { }

  login(model:any){
    return this.http.post(this.baseUrl + 'account/login', model).pipe(
      map((response : user) =>{
        const user = response;
         if(response){
          this.setCurrentUser(user);
          this.presence.createHubConnection(user);
        }
         return user;
      })
    )}

    register(model:any){
      return this.http.post(this.baseUrl + 'account/register',model).pipe(
        map((user: user) =>{
          if(user){
            this.setCurrentUser(user);
            this.presence.createHubConnection(user);
          }
        })
      )
    }

    setCurrentUser(user : user){
      user.roles = [];
      const roles = this.getDecodedToken(user.token).role;
      Array.isArray(roles) ? user.roles = roles:user.roles.push(roles);
      localStorage.setItem('user', JSON.stringify(user));
      this.currenyUserSource.next(user);
    }

    logout(){
      localStorage.removeItem('user');
      this.currenyUserSource.next(null);
      this.presence.stopHubConnection();
    }

    getDecodedToken(token){
        return JSON.parse(atob(token.split('.')[1]));
    }
  }
