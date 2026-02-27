import { Injectable } from '@angular/core';
import ApiRepository from './api-repository';
import { HttpClient } from '@angular/common/http';
import environments from '../../environments';
import DevicesTypesResponseModel from '../services/device/models/DevicesTypesResponseModel';
import DevicesResponseModel from '../services/device/models/DevicesResponseModel';
import { Observable } from 'rxjs';
import CameraCreateModel from '../services/device/models/CameraCreateModel';
import SensorCreateModel from '../services/device/models/SensorCreateModel';
import LampCreateModel from '../services/device/models/LampCreateModel';
import ImporterBasicInfoModel from '../services/device/models/ImporterBasicInfoModel';
import ImportDevicesModel from '../services/device/models/ImportDevicesModel';

@Injectable({
  providedIn: 'root'
})
export class DeviceApiRepositoryService extends ApiRepository{

  constructor(http: HttpClient) {
    super(environments.smartHomeApi, http);
   }
  
  public getDevicesTypes(){
    return this.get<Array<DevicesTypesResponseModel>>("dispositivos/tipos");
  }

  public getDevices(
    pageNumber: number,
    pageSize: number,
    deviceName: string,
    modelName: string,
    companyName: string,
    deviceType: string
  ):Observable<DevicesResponseModel>{
    const query = `numeroDePagina=${pageNumber}&tamañoDePagina=${pageSize}&nombreDispositivo=${deviceName}&modelo=${modelName}&nombreEmpresa=${companyName}&tipoDispositivo=${deviceType}`;
    return this.get<DevicesResponseModel>("dispositivos", query);
  }

  public createCamera(
    credentials: CameraCreateModel
  ){
    return this.post(credentials, "camaras");
  }

  public createMovementSensor(
    credentials: SensorCreateModel
  ){
    return this.post(credentials, "sensores-movimiento");
  }

  public createWindowSensor(
    credentials: SensorCreateModel
  ){
    return this.post(credentials, "sensores-ventana");
  }
  
  public createLamp(
    credentials: LampCreateModel
  ){
    return this.post(credentials, "lamparas");
  }

  public getImporters(): Observable<Array<ImporterBasicInfoModel>>{
    return this.get<Array<ImporterBasicInfoModel>>("importadores");
  }

  public importDevices(
    values: ImportDevicesModel
  ){
    return this.post(values, "importadores/dispositivos");
  }
}
