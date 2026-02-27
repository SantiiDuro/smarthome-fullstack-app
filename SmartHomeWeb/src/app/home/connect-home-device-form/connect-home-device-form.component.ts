import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HomeDeviceService } from '../../../backend/services/home-device/home-device.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-connect-home-device-form',
  templateUrl: './connect-home-device-form.component.html',
  styles: ``
})
export class ConnectHomeDeviceFormComponent {
  selectedHomeDevice: string | null = null;

  readonly formField: any = {
    homeDevice: {
      name: "homeDevice",
      required: "Dispositivo es requerido",
    },
  };

  readonly connectHomeDeviceForm = new FormGroup({
    [this.formField.homeDevice.name]: new FormControl("", [
      Validators.required,
    ])
  });

  connectStatus: {
    loading?: true;
    error?: string;
  } | null = null;

  constructor(
    private readonly _router: Router,
    private readonly _homeDeviceService: HomeDeviceService
  ) { }

  public onSubmit(){
    this.connectStatus = { loading: true };

    const formValues = this.connectHomeDeviceForm.value;
    
    this._homeDeviceService.connect(this.selectedHomeDevice || '').subscribe({
      next: (response) => {
        this.connectStatus = null;

        this._router.navigate(['/home', 'myHomes']);
      },
      error: (error) => {
        this.connectStatus = { error };
      }
    });
  }
}
