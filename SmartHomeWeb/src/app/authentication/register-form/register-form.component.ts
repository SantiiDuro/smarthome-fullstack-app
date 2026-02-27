import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import HomeOwnerRegisterModel from '../../../backend/services/user/models/HomeOwnerRegisterModel';
import { UserService } from '../../../backend/services/user/user.service';

@Component({
  selector: 'app-register-form',
  templateUrl: './register-form.component.html',
  styles: ``
})
export class RegisterFormComponent {
  readonly formField: any = {
    nombre: {
      name: "nombre",
      required: "Nombre es requerido",
    },
    apellido: {
      name: "apellido",
      required: "Apellido es requerido",
    },
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
    fotoPerfil: {
      name: "fotoPerfil",
      required: "Foto de perfil es requerida",
    }
  };

  get contrasenaField() {
    return this.formField.contraseña;
  }

  readonly registerForm = new FormGroup({
    [this.formField.nombre.name]: new FormControl("", [
      Validators.required
    ]),
    [this.formField.apellido.name]: new FormControl("", [
      Validators.required
    ]),
    [this.formField.email.name]: new FormControl("", [
      Validators.required,
      Validators.email,
    ]),
    [this.formField.contraseña.name]: new FormControl("", [
      Validators.required,
      Validators.minLength(6),
    ]),
    [this.formField.fotoPerfil.name]: new FormControl("", [
      Validators.required
    ])
  });

  registerStatus: {
    loading?: true;
    error?: string;
  } | null = null;

  constructor(
    private readonly _router: Router,
    private readonly _userService: UserService
  ) { }

  public onSubmit(values: HomeOwnerRegisterModel) {
    this.registerStatus = { loading: true };

    this._userService.registerHomeOwner(values).subscribe({
      next: (response) => {
        this.registerStatus = null;

        this._router.navigate(["/login"]);
      },
      error: (error) => {
        this.registerStatus = { error };
      },
    });
  }
}
