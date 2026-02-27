import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import environments from '../../environments';
import ApiRepository from './api-repository';
import AssignDeviceRoomModel from '../services/home-room/models/AssignDeviceRoomModel';

@Injectable({
  providedIn: 'root'
})
export class HomeRoomApiRepositoryService extends ApiRepository{

  constructor(http: HttpClient) {
    super(environments.smartHomeApi, http)
  }

  public addDevice(
    roomId: string,
    credentials: AssignDeviceRoomModel,
  ){
    return this.post(credentials, `cuartos/${roomId}/dispositivos-hogar`);
  }
}
