import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-resetpassword',
  templateUrl: './resetpassword.component.html',
  styleUrls: ['./resetpassword.component.css']
})
export class ResetpasswordComponent implements OnInit {

  resetPWForm : FormGroup;
  _token:string;
  _email:string;
  validationErrors:string[]=[];

  constructor(private accountService: AccountService, private router: Router, 
              private route:ActivatedRoute,private fb :FormBuilder) {

                console.log(this.route.snapshot.params['email']);
                console.log(this.route.snapshot.params['token']);

               }

  ngOnInit(): void {
    this.initializeForm();
  }

  register(){
    this._email=this.route.snapshot.queryParams['email'];
    this._token=this.route.snapshot.queryParams['token'];
    console.log(this._token, this._email);
  }

  initializeForm(){
    this.resetPWForm = this.fb.group({
      password : ['',[Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword : ['',[Validators.required, this.matchValues('password')]]
    })
}
matchValues(matchTo:string): ValidatorFn { 
  return (control :AbstractControl) =>{
    return control?.value === control?.parent?.controls[matchTo].value ? null : {isMatching : true}
  }
}

}
