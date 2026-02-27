import { Injectable } from '@angular/core';
import { DeviceApiRepositoryService } from '../../repositories/device-api-repository.service';
import { Observable } from 'rxjs';
import DevicesTypesResponseModel from './models/DevicesTypesResponseModel';
import DevicesResponseModel from './models/DevicesResponseModel';
import CameraCreateModel from './models/CameraCreateModel';
import SensorCreateModel from './models/SensorCreateModel';
import LampCreateModel from './models/LampCreateModel';
import ImporterBasicInfoModel from './models/ImporterBasicInfoModel';
import ImportDevicesModel from './models/ImportDevicesModel';

@Injectable({
  providedIn: 'root'
})
export class DeviceService {

  constructor(private readonly _repository: DeviceApiRepositoryService) { }

  public getDevicesTypes(): Observable<Array<DevicesTypesResponseModel>>{
    return this._repository.getDevicesTypes();
  }

  public getDevices(
    pageNumber: number,
    pageSize: number,
    deviceName = '',
    modelName = '',
    companyName = '',
    deviceType = ''
  ): Observable<DevicesResponseModel>{
    return this._repository.getDevices(pageNumber, pageSize, deviceName, modelName, companyName, deviceType);
  }

  public createCamera(
    credentials: CameraCreateModel
  ){
    return this._repository.createCamera(credentials);
  }

  public createMovementSensor(
    credentials: SensorCreateModel
  ){
    return this._repository.createMovementSensor(credentials);
  }

  public createWindowSensor(
    credentials: SensorCreateModel
  ){
    return this._repository.createWindowSensor(credentials);
  }
  
  public createLamp(
    credentials: LampCreateModel
  ){
    return this._repository.createLamp(credentials);
  }

  public getImporters(): Observable<Array<ImporterBasicInfoModel>>{
    return this._repository.getImporters();
  }

  public importDevices(
    values: ImportDevicesModel
  ){
    return this._repository.importDevices(values);
  }
}
