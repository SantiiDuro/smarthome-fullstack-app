import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { DeviceService } from '../../../backend/services/device/device.service';
import SensorCreateModel from '../../../backend/services/device/models/SensorCreateModel';

@Component({
  selector: 'app-create-sensor-form',
  templateUrl: './create-sensor-form.component.html',
  styles: ``
})
export class CreateSensorFormComponent {
  sensorType: string | null = null;
  
  readonly formField: any = {
    nombre: {
      name: "nombre",
      required: "Nombre es requerido",
    },
    modelo: {
      name: "modelo",
      required: "Modelo es requerido",
    },
    descripcion: {
      name: "descripcion",
      required: "Descripción es requerida",
    },
    fotografiaPrincipal: {
      name: "fotografiaPrincipal",
      required: "Fotografía principal es requerida",
    },
    tipoSensor: {
      name: "tipoSensor",
      required: "Tipo de sensor es requerido",
    },
  };

  readonly createSensorForm = new FormGroup({
    [this.formField.nombre.name]: new FormControl("", [
      Validators.required
    ]),
    [this.formField.modelo.name]: new FormControl("", [
      Validators.required
    ]),
    [this.formField.descripcion.name]: new FormControl("", [
      Validators.required,
    ]),
    [this.formField.fotografiaPrincipal.name]: new FormControl("", [
      Validators.required,
    ]),
    [this.formField.tipoSensor.name]: new FormControl("", [
      Validators.required,
    ])
  });

  secondaryPhotos: string[] = [];

  createStatus: {
    loading?: true;
    error?: string;
  } | null = null;

  constructor(
    private readonly _router: Router,
    private readonly _deviceService: DeviceService
  ) { }

  onSecondaryPhotosChange(photos: string[]) {
    this.secondaryPhotos = photos;
  }

  public onSubmit() {
    this.createStatus = { loading: true };

    const formValues = this.createSensorForm.value;
    const model: SensorCreateModel = {
      nombre: formValues[this.formField.nombre.name] || '',
      modelo: formValues[this.formField.modelo.name] || '',
      descripcion: formValues[this.formField.descripcion.name] || '',
      fotografias: [
        { url: formValues[this.formField.fotografiaPrincipal.name] || '', esPrincipal: true },
        ...this.secondaryPhotos.map(url => ({ url, esPrincipal: false }))
      ],
    };

    if (formValues[this.formField.tipoSensor.name] == "movimiento") {
      this._deviceService.createMovementSensor(model).subscribe({
        next: (response) => {
          this.createStatus = null;
  
          this._router.navigate(["/home"]);
        },
        error: (error) => {
          this.createStatus = { error };
        },
      });
    }
    else if (formValues[this.formField.tipoSensor.name] == "ventana") {
      this._deviceService.createWindowSensor(model).subscribe({
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
}
