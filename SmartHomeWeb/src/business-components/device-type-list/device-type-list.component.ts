import { Component } from '@angular/core';
import { ListComponent } from '../../components/list/list.component';
import { Subscription } from 'rxjs/internal/Subscription';
import { CommonModule, NgIf } from '@angular/common';
import DeviceTypeStatus from './models/device-type.status';
import { DeviceService } from '../../backend/services/device/device.service';

@Component({
  selector: 'app-device-type-list',
  standalone: true,
  imports: [ListComponent, NgIf, CommonModule],
  templateUrl: './device-type-list.component.html',
  styles: ``
})
export class DeviceTypeListComponent {
  status: DeviceTypeStatus = {
    loading: true,
    devicesTypes: []
  }

  private _companyGetAllSubscription: Subscription | null = null;
  
  constructor(private readonly _devicesService: DeviceService){}

  ngOnDestroy() : void {
    this._companyGetAllSubscription?.unsubscribe();
  }

  ngOnInit() : void {
    this._companyGetAllSubscription = this._devicesService
      .getDevicesTypes()
      .subscribe({
        next: (devicesTypes) => {
          this.status = {
            devicesTypes: devicesTypes.map(deviceType => ({ label: deviceType.tipo })),
          };
        },
        error: (error) => {
          this.status ={
            devicesTypes: [],
            error
          };
        }
      });
  }
}
