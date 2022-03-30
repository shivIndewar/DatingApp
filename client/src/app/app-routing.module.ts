import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { EmailconfirmationComponent } from './emailconfirmation/emailconfirmation.component';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { ForgotpasswordComponent } from './forgotpassword/forgotpassword.component';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { RegisterComponent } from './register/register.component';
import { ResetpasswordComponent } from './resetpassword/resetpassword.component';
import { AdminGuard } from './_guards/admin.guard';
import { AuthGuard } from './_guards/auth.guard';
import { PreventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';
import { MemberDetailedResolver } from './_resolvers/member-detailed-resolver';

const routes: Routes = [
  {path:'',component: HomeComponent},
  {
    path :'',
    runGuardsAndResolvers:'always',
    canActivate:[AuthGuard],
    children:[
  {path:'members',component: MemberListComponent},
  {path:'members/:username',component: MemberDetailComponent, resolve:{member:MemberDetailedResolver}},
  {path:'member/edit',component: MemberEditComponent, canDeactivate:[PreventUnsavedChangesGuard]},
  {path:'lists',component: ListsComponent},
  {path:'messages',component: MessagesComponent},
  {path:'admin',component: AdminPanelComponent, canActivate: [AdminGuard]},

    ]
  },

  {path:'emailconfirmation', component: EmailconfirmationComponent},
  {path:'resetpassword/:email/:token', component: ResetpasswordComponent},
  {path:'register', component: RegisterComponent},
  {path:'forgotPassword', component: ForgotpasswordComponent},
  {path:'errors', component:TestErrorsComponent},
  {path:'**',component: HomeComponent, pathMatch:'full'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
