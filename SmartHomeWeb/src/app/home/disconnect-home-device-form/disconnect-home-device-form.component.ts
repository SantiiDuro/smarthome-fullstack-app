import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { HomeDeviceService } from '../../../backend/services/home-device/home-device.service';

@Component({
  selector: 'app-disconnect-home-device-form',
  templateUrl: './disconnect-home-device-form.component.html',
  styles: ``
})
export class DisconnectHomeDeviceFormComponent {
  selectedHomeDevice: string | null = null;

  readonly formField: any = {
    homeDevice: {
      name: "homeDevice",
      required: "Dispositivo es requerido",
    },
  };

  readonly disconnectHomeDeviceForm = new FormGroup({
    [this.formField.homeDevice.name]: new FormControl("", [
      Validators.required,
    ])
  });

  disconnectStatus: {
    loading?: true;
    error?: string;
  } | null = null;

  constructor(
    private readonly _router: Router,
    private readonly _homeDeviceService: HomeDeviceService
  ) { }

  public onSubmit(){
    this.disconnectStatus = { loading: true };

    const formValues = this.disconnectHomeDeviceForm.value;
    
    this._homeDeviceService.disconnect(this.selectedHomeDevice || '').subscribe({
      next: (response) => {
        this.disconnectStatus = null;

        this._router.navigate(['/home', 'myHomes']);
      },
      error: (error) => {
        this.disconnectStatus = { error };
      }
    });
  }
}
