import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuthenticationRoutingModule } from './authentication-routing.module';
import { AuthenticationPageComponent } from './authentication-page/authentication-page.component';
import { FormComponent } from '../../components/form-elements/form/form.component';
import { LoginFormComponent } from './login-form/login-form.component';
import { FormInputComponent } from '../../components/form-elements/form-input/form-input.component';
import { FormButtonComponent } from '../../components/form-elements/form-button/form-button.component';
import { RouterModule } from '@angular/router';
import { ButtonComponent } from '../../components/button/button.component';
import { RegisterFormComponent } from './register-form/register-form.component';

@NgModule({
  declarations: [AuthenticationPageComponent, LoginFormComponent, RegisterFormComponent],
  imports: [CommonModule, AuthenticationRoutingModule, FormComponent, FormInputComponent, FormButtonComponent, RouterModule, ButtonComponent]
})
export class AuthenticationModule { }
