import { Injectable } from '@angular/core';
import ApiRepository from './api-repository';
import { HttpClient } from '@angular/common/http';
import environments from '../../environments';
import ModifyHomeDeviceNameModel from '../services/home-device/models/ModifyHomeDeviceNameModel';

@Injectable({
  providedIn: 'root'
})
export class HomeDeviceApiRepositoryService extends ApiRepository {

  constructor(http: HttpClient) {
    super(environments.smartHomeApi, http);
  }

  public modifyName(
    credentials: ModifyHomeDeviceNameModel,
    homeDeviceId: string
  ){
    return this.patch(credentials, `dispositivos-hogar/${homeDeviceId}/nombre`);
  }

  public connect(homeDeviceId: string){
    return this.post({}, `dispositivos-hogar/${homeDeviceId}/conectar`);
  }

  public disconnect(homeDeviceId: string){
    return this.post({}, `dispositivos-hogar/${homeDeviceId}/desconectar`);
  }
}
