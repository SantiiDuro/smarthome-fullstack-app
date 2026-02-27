import { Component } from '@angular/core';
import { TableComponent } from '../../components/table/table.component';
import { CommonModule } from '@angular/common';
import { FormComponent } from '../../components/form-elements/form/form.component';
import { FormInputComponent } from '../../components/form-elements/form-input/form-input.component';
import { FormButtonComponent } from '../../components/form-elements/form-button/form-button.component';
import { FormControl, FormGroup } from '@angular/forms';
import { DeviceService } from '../../backend/services/device/device.service';
import DeviceBasicInfoModel from '../../backend/services/device/models/DeviceBasicInfoModel';

@Component({
  selector: 'app-device-table',
  standalone: true,
  imports: [TableComponent, CommonModule, FormComponent, FormInputComponent, FormButtonComponent],
  templateUrl: './device-table.component.html',
  styles: ``
})
export class DeviceTableComponent {
  devices: Array<DeviceBasicInfoModel> = [];
  currentPage = 1;
  pageSize = 10;
  totalPages = 0;

  readonly formField: any = {
    nombreDispositivo: {
      name: "nombreDispositivo"
    },
    modelo: {
      name: "modelo"
    },
    nombreEmpresa: {
      name: "nombreEmpresa"
    },
    tipoDispositivo: {
      name: "tipoDispositivo"
    },
  };

  readonly filterForm = new FormGroup({
    [this.formField.nombreDispositivo.name]: new FormControl("", []),
    [this.formField.modelo.name]: new FormControl("", []),
    [this.formField.nombreEmpresa.name]: new FormControl("", []),
    [this.formField.tipoDispositivo.name]: new FormControl("", []),
    
  });

  constructor(
    private readonly _deviceService: DeviceService,
  ) {}

  ngOnInit() {
    this.loadDevices();
  }

  loadDevices() {
    const nombreDispositivo = this.filterForm.get('nombreDispositivo')?.value;
    const modelo = this.filterForm.get('modelo')?.value;
    const nombreEmpresa = this.filterForm.get('nombreEmpresa')?.value;
    const tipoDispositivo = this.filterForm.get('tipoDispositivo')?.value;

    this._deviceService.getDevices(this.currentPage, this.pageSize, nombreDispositivo, modelo, nombreEmpresa, tipoDispositivo).subscribe({
      next: (response) => {
        this.devices = response.dispositivos;
        this.totalPages = response.cantidadPaginas;
      },
      error: (error) => {
        console.error('Error loading devices', error);
      }
    });
  }

  onFilterSubmit() {
    this.currentPage = 1;
    this.loadDevices();
  }

  goToPreviousPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadDevices();
    }
  }

  goToNextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadDevices();
    }
  }
}
