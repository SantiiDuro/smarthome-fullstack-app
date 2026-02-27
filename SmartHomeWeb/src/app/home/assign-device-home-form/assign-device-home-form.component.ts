import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HomeService } from '../../../backend/services/home/home.service';
import AssignDeviceModel from '../../../backend/services/home/models/AssignDeviceModel';

@Component({
  selector: 'app-assign-device-home-form',
  templateUrl: './assign-device-home-form.component.html',
  styles: ``
})
export class AssignDeviceHomeFormComponent {
  selectedDevice: string | null = null;
  selectedHome: string | null = null;

  readonly formField: any = {
    device: {
      name: "device",
      required: "Dispositivo es requerido",
    }
  };

  readonly assignDeviceHomeForm = new FormGroup({
    [this.formField.device.name]: new FormControl("", [
      Validators.required,
    ])
  });

  assignStatus: {
    loading?: true;
    error?: string;
  } | null = null;

  constructor(
    private readonly _router: Router,
    private readonly _route: ActivatedRoute,
    private readonly _homeService: HomeService
  ) { }

  ngOnInit(): void {
    this._route.params.subscribe((params) => {
      this.selectedHome = params['homeId'] || null;

      if (!this.selectedHome) {
        this._router.navigate(['/home', 'myHomes']);
      }
    });
  }

  public onSubmit(){
    this.selectedDevice;
    this.assignStatus = { loading: true };
    
    const model: AssignDeviceModel = {
      dispositivoId: this.selectedDevice || '',
    }

    this._homeService.addDevice(this.selectedHome || '', model).subscribe({
      next: (response) => {
        this.assignStatus = null;

        this._router.navigate(['/home', 'myHomes']);
      },
      error: (error) => {
        this.assignStatus = { error };
      }
    });
  }
}
