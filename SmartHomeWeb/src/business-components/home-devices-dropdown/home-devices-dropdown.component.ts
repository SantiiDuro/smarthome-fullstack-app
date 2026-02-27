import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule, NgIf } from '@angular/common';
import HomeDeviceStatus from './models/home-device.status';
import { Subscription } from 'rxjs';
import { HomeService } from '../../backend/services/home/home.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup } from '@angular/forms';
import { FormDropdownComponent } from '../../components/form-elements/form-dropdown/form-dropdown.component';

@Component({
  selector: 'app-home-devices-dropdown',
  standalone: true,
  imports: [FormDropdownComponent, CommonModule, NgIf],
  templateUrl: './home-devices-dropdown.component.html',
  styles: ``
})
export class HomeDevicesDropdownComponent {
  @Input({ required: true }) name!: string;
  @Input({ required: true }) form!: FormGroup;
  @Input({ required: true }) formField!: any;
  @Input({ required: true }) value: string | null = null;
  @Output() valueChange = new EventEmitter<string | null>();

  selectedHome: string | null = null;
  
  status: HomeDeviceStatus = {
    loading: true,
    devices: [],
  }

  private _homeDevicesGetAllSubscription: Subscription | null = null;

  constructor(
    private readonly _homeService: HomeService,
    private readonly _router: Router,
    private readonly _route: ActivatedRoute
  ) {}

  ngOnDestroy(): void {
    this._homeDevicesGetAllSubscription?.unsubscribe();
  }

  ngOnInit(): void {
    this._route.params.subscribe((params) => {
      this.selectedHome = params['homeId'] || null;

      if (!this.selectedHome) {
        this._router.navigate(['/home', 'myHomes']);
      }

      this._homeDevicesGetAllSubscription = this._homeService
      .getHomeDevices("", this.selectedHome || "")
      .subscribe({
        next: (devices) => {
          this.status = {
            devices: devices.map((device) => ({
              value: device.id,
              label: `Empresa: ${device.nombreEmpresa}, Nombre: ${device.nombre}, Modelo: ${device.modelo}`,
            })),
          };
        },
        error: (error) => {
          this.status = {
            devices: [],
            error,
          };
        },
      });
    });
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
