import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl, FormControl } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-forgotpassword',
  templateUrl: './forgotpassword.component.html',
  styleUrls: ['./forgotpassword.component.css']
})
export class ForgotpasswordComponent implements OnInit {
  forgotPasswordForm : FormGroup;
  constructor(private accountService : AccountService, private fb :FormBuilder, private router: Router, private toastr: ToastrService) { }
  notificationsSent : boolean = false;
  ngOnInit(): void {
    this.initializeForm();
  }

  forgotPassword(){
    this.accountService.forgotPassword(this.forgotPasswordForm.value.email).subscribe(response =>{
      this.notificationsSent = (response==true);
      this.forgotPasswordForm.reset();
    }, error =>{
      this.toastr.error(error.error);
    })
  }

  initializeForm(){
    this.forgotPasswordForm = this.fb.group({
      email : ['',Validators.required],
    })
  }
}
