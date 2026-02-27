import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import ModifyHomeDeviceNameModel from '../../../backend/services/home-device/models/ModifyHomeDeviceNameModel';
import { HomeDeviceService } from '../../../backend/services/home-device/home-device.service';

@Component({
  selector: 'app-modify-home-device-name-form',
  templateUrl: './modify-home-device-name-form.component.html',
  styles: ``
})
export class ModifyHomeDeviceNameFormComponent {
  selectedHomeDevice: string | null = null;

  readonly formField: any = {
    nombre: {
      name: "nombre",
      required: "Nombre es requerido",
    },
    homeDevice: {
      name: "homeDevice",
      required: "Dispositivo es requerido",
    },
  };

  readonly modifyHomeDeviceNameForm = new FormGroup({
    [this.formField.nombre.name]: new FormControl("", [
      Validators.required
    ]),
    [this.formField.homeDevice.name]: new FormControl("", [
      Validators.required,
    ])
  });

  modifyStatus: {
    loading?: true;
    error?: string;
  } | null = null;

  constructor(
    private readonly _router: Router,
    private readonly _homeDeviceService: HomeDeviceService
  ) { }

  public onSubmit(){
    this.modifyStatus = { loading: true };

    const formValues = this.modifyHomeDeviceNameForm.value;

    const model: ModifyHomeDeviceNameModel = {
      nombre: formValues[this.formField.nombre.name] || '',
    }
    
    this._homeDeviceService.modifyName(model, this.selectedHomeDevice || '').subscribe({
      next: (response) => {
        this.modifyStatus = null;

        this._router.navigate(['/home', 'myHomes']);
      },
      error: (error) => {
        this.modifyStatus = { error };
      }
    });
  }
}
