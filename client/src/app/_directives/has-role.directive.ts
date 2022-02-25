import { Directive, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { take } from 'rxjs/operators';
import { user } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Directive({
  selector: '[appHasRole]'
})
export class HasRoleDirective implements OnInit {

  @Input() appHasRole :string[];
  user : user;

  constructor(private viewContainerRef : ViewContainerRef, private tempateRef : TemplateRef<any>, 
      private accountService: AccountService) { 
        this.accountService.currentUser$.pipe(take(1)).subscribe(user =>{
          console.log("called has role constructor");
          this.user = user;
        })
      }
  ngOnInit(): void {
    //clear view if no roles

    if(this.user?.roles || this.user == null){
      this.viewContainerRef.clear();
    }
    if(this.user?.roles.some(r  => this.appHasRole.includes(r))){
      this.viewContainerRef.createEmbeddedView(this.tempateRef);
    }else{
      this.viewContainerRef.clear();
    }
  }

}