import { NgIf } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { FormDropdownComponent } from '../../components/form-elements/form-dropdown/form-dropdown.component';
import DeviceStatus from './models/device.status';
import DeviceBasicInfoModel from '../../backend/services/device/models/DeviceBasicInfoModel';
import { DeviceService } from '../../backend/services/device/device.service';
import { Subscription } from 'rxjs';
import { FormComponent } from '../../components/form-elements/form/form.component';
import { FormInputComponent } from '../../components/form-elements/form-input/form-input.component';
import { FormButtonComponent } from '../../components/form-elements/form-button/form-button.component';

@Component({
  selector: 'app-device-dropdown',
  standalone: true,
  imports: [NgIf, FormDropdownComponent, FormComponent, FormInputComponent, FormButtonComponent],
  templateUrl: './device-dropdown.component.html',
  styles: ``
})
export class DeviceDropdownComponent {
  @Input({ required: true }) name!: string;
  @Input({ required: true }) form!: FormGroup;
  @Input({ required: true }) formField!: any;
  @Input({ required: true }) value: string | null = null;
  @Output() valueChange = new EventEmitter<string | null>();

  status: DeviceStatus = {
    loading: true,
    devices: [],
  }

  devices: Array<DeviceBasicInfoModel> = [];
  currentPage = 1;
  pageSize = 5;
  totalPages = 0;

  readonly filterFormField: any = {
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
    [this.filterFormField.nombreDispositivo.name]: new FormControl("", []),
    [this.filterFormField.modelo.name]: new FormControl("", []),
    [this.filterFormField.nombreEmpresa.name]: new FormControl("", []),
    [this.filterFormField.tipoDispositivo.name]: new FormControl("", []),
    
  });

  private _devicesGetAllSubscription: Subscription | null = null;

  constructor(
    private readonly _deviceService: DeviceService,
  ) {}

  ngOnDestroy(): void {
    this._devicesGetAllSubscription?.unsubscribe();
  }

  ngOnInit() {
    this.loadDevices();
  }

  loadDevices() {
    const nombreDispositivo = this.filterForm.get('nombreDispositivo')?.value;
    const modelo = this.filterForm.get('modelo')?.value;
    const nombreEmpresa = this.filterForm.get('nombreEmpresa')?.value;
    const tipoDispositivo = this.filterForm.get('tipoDispositivo')?.value;

    this._devicesGetAllSubscription = this._deviceService
    .getDevices(this.currentPage, this.pageSize, nombreDispositivo, modelo, nombreEmpresa, tipoDispositivo).subscribe({
      next: (devices) => {
        this.devices = devices.dispositivos;
        this.totalPages = devices.cantidadPaginas;
        this.status = {
          devices: devices.dispositivos.map((device) => ({
            value: device.id,
            label: `Empresa: ${device.nombreEmpresa}, Nombre: ${device.nombre}, Modelo: ${device.modelo}`,
          })),
        };
      },
      error: (error) => {
        console.error('Error loading devices', error);
      }
    });
  }

  onFilterSubmit() {
    this.currentPage = 1;
    this.value = null;
    this.onValueChange(this.value);
    this.loadDevices();
  }

  goToPreviousPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.value = null;
      this.onValueChange(this.value);
      this.loadDevices();
    }
  }

  goToNextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.value = null;
      this.onValueChange(this.value);
      this.loadDevices();
    }
  }

  public onChange(event: any): void {
    const newValue = event.target.value === 'null' ? null : event.target.value;
    this.valueChange.emit(newValue);

    this.form.get(this.name)?.setValue(newValue);
  }
  
  public onValueChange(newValue: string | null) {
    this.valueChange.emit(newValue);
    this.form.get(this.name)?.setValue(newValue);
  }
}
