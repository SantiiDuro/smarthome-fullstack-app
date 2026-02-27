import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { HomeService } from '../../../backend/services/home/home.service';
import HomeCreateModel from '../../../backend/services/home/models/HomeCreateModel';

@Component({
  selector: 'app-create-home-form',
  templateUrl: './create-home-form.component.html',
  styles: ``
})
export class CreateHomeFormComponent {
  readonly formField: any = {
    calle: {
      name: "calle",
      required: "Calle es requerida",
    },
    numPuerta: {
      name: "numPuerta",
      required: "Número de puerta es requerido",
      min: "Número de puerta debe ser mayor o igual a 0",
    },
    latitud: {
      name: "latitud",
      required: "Latitud es requerida",
      min: "Latitud debe ser mayor o igual a -90",
      max: "Latitud debe ser menor o igual a 90",
    },
    longitud: {
      name: "longitud",
      required: "Longitud es requerida",
      min: "Longitud debe ser mayor o igual a -180",
      max: "Longitud debe ser menor o igual a 180",
    },
    cantMiembrosSoportados: {
      name: "cantMiembrosSoportados",
      required: "Cantidad de miembros soportados es requerido",
      min: "Cantidad de miembros soportados debe ser mayor o igual a 1",
    },
    alias: {
      name: "alias",
    },
  };

  readonly createHomeForm = new FormGroup({
    [this.formField.calle.name]: new FormControl("", [
      Validators.required
    ]),
    [this.formField.numPuerta.name]: new FormControl<number | null>(null, [
      Validators.required,
      Validators.min(0),
    ]),
    [this.formField.latitud.name]: new FormControl<number | null>(null, [
      Validators.required,
      Validators.min(-90),
      Validators.max(90),
    ]),
    [this.formField.longitud.name]: new FormControl<number | null>(null, [
      Validators.required,
      Validators.min(-180),
      Validators.max(180),
    ]),
    [this.formField.cantMiembrosSoportados.name]: new FormControl<number | null>(null, [
      Validators.required,
      Validators.min(1),
    ]),
    [this.formField.alias.name]: new FormControl(""),
  });

  createStatus: {
    loading?: true;
    error?: string;
  } | null = null;

  constructor(
    private readonly _router: Router,
    private readonly _homeService: HomeService
  ) { }

  public onSubmit(values: HomeCreateModel) {
    this.createStatus = { loading: true };

    const processedValues = {
      ...values,
      alias: values.alias === "" ? null : values.alias,
    };

    this._homeService.createHome(processedValues).subscribe({
      next: (response) => {
        this.createStatus = null;

        this._router.navigate(["/home"]);
      },
      error: (error) => {
        this.createStatus = { error };
      },
    });
  }
}
