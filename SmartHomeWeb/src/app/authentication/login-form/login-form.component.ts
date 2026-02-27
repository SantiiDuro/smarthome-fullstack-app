import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import UserCredentialsModel from '../../../backend/services/session/models/UserCredentialsModel';
import { Router } from '@angular/router';
import { SessionService } from '../../../backend/services/session/session.service';

@Component({
  selector: "app-login-form",
  templateUrl: "./login-form.component.html",
  styles: ``,
})
export class LoginFormComponent {
  readonly formField: any = {
    email: {
      name: "email",
      required: "Email es requerido",
      email: "Email no es valido",
    },
    contraseña: {
      name: "contraseña",
      required: "Contraseña es requerida",
      minlength: "Contraseña debe tener al menos 6 caracteres",
    },
  };

  get contrasenaField() {
    return this.formField.contraseña;
  }

  readonly loginForm = new FormGroup({
    [this.formField.email.name]: new FormControl("", [
      Validators.required,
      Validators.email,
    ]),
    [this.formField.contraseña.name]: new FormControl("", [
      Validators.required,
      Validators.minLength(6),
    ]),
  });

  loginStatus: {
    loading?: true;
    error?: string;
  } | null = null;

  constructor(
    private readonly _router: Router,
    private readonly _sessionService: SessionService
  ) { }

  public onSubmit(values: UserCredentialsModel) {
    this.loginStatus = { loading: true };

    this._sessionService.login(values).subscribe({
      next: (response) => {
        this.loginStatus = null;

        this._router.navigate(["/home"]);
      },
      error: (error) => {
        this.loginStatus = { error };
      },
    });
  }
}
