import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { DeviceService } from '../../../backend/services/device/device.service';
import CameraCreateModel from '../../../backend/services/device/models/CameraCreateModel';

@Component({
  selector: 'app-create-camera-form',
  templateUrl: './create-camera-form.component.html',
  styles: ``
})
export class CreateCameraFormComponent {
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
  };

  readonly createCameraForm = new FormGroup({
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
    ])
  });

  secondaryPhotos: string[] = [];

  checkboxOptions = {
    detectaMovimiento: false,
    detectaPersona: false,
    usoExterior: false,
    usoInterior: false,
  };

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

  onCheckboxOptionsChange(options: { 
    detectaMovimiento: boolean; 
    detectaPersona: boolean; 
    usoExterior: boolean; 
    usoInterior: boolean; 
  }) {
    this.checkboxOptions = options;
  }

  public onSubmit() {
    this.createStatus = { loading: true };

    const formValues = this.createCameraForm.value;
    const model: CameraCreateModel = {
      nombre: formValues[this.formField.nombre.name] || '',
      modelo: formValues[this.formField.modelo.name] || '',
      descripcion: formValues[this.formField.descripcion.name] || '',
      fotografias: [
        { url: formValues[this.formField.fotografiaPrincipal.name] || '', esPrincipal: true },
        ...this.secondaryPhotos.map(url => ({ url, esPrincipal: false }))
      ],
      detectaMovimiento: this.checkboxOptions.detectaMovimiento,
      detectaPersona: this.checkboxOptions.detectaPersona,
      usoExterior: this.checkboxOptions.usoExterior,
      usoInterior: this.checkboxOptions.usoInterior,
    };

    this._deviceService.createCamera(model).subscribe({
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
