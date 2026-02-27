import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService } from '../../../backend/services/user/user.service';
import BORegisterModel from '../../../backend/services/user/models/BORegisterModel';

@Component({
  selector: 'app-create-bo-form',
  templateUrl: './create-bo-form.component.html',
  styles: ``
})
export class CreateBOFormComponent {
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
  };

  get contrasenaField() {
    return this.formField.contraseña;
  }

  readonly createBOForm = new FormGroup({
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

  public onSubmit(values: BORegisterModel) {
    this.registerStatus = { loading: true };

    this._userService.registerBO(values).subscribe({
      next: (response) => {
        this.registerStatus = null;

        this._router.navigate(["/home"]);
      },
      error: (error) => {
        this.registerStatus = { error };
      },
    });
  }
}
