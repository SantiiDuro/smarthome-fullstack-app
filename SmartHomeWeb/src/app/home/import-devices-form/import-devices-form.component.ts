import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { DeviceService } from '../../../backend/services/device/device.service';
import ImportDevicesModel from '../../../backend/services/device/models/ImportDevicesModel';

@Component({
  selector: 'app-import-devices-form',
  templateUrl: './import-devices-form.component.html',
  styles: ``
})
export class ImportDevicesFormComponent {
  selectedImporter: string | null = null;

  readonly formField: any = {
    identificadorImportador: {
      name: "identificadorImportador",
      required: "Importador es requerido",
    },
    ruta: {
      name: "ruta",
      required: "Ruta es requerida",
    },
  };

  readonly importDevicesForm = new FormGroup({
    [this.formField.identificadorImportador.name]: new FormControl("", [
      Validators.required,
    ]),
    [this.formField.ruta.name]: new FormControl("", [
      Validators.required,
    ]),
  });

  importStatus: {
    loading?: true;
    error?: string;
  } | null = null;

  constructor(
    private readonly _router: Router,
    private readonly _deviceService: DeviceService
  ) { }

  public onSubmit(values: ImportDevicesModel){
    this.importStatus = { loading: true };
    
    this._deviceService.importDevices(values).subscribe({
      next: (response) => {
        this.importStatus = null;

        this._router.navigate(["/home"]);
      },
      error: (error) => {
        this.importStatus = { error };
      }
    });
  }
}
