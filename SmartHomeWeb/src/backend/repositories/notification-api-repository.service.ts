import { Injectable } from '@angular/core';
import ApiRepository from './api-repository';
import { HttpClient } from '@angular/common/http';
import environments from '../../environments';
import { Observable } from 'rxjs';
import NotificationBasicInfoModel from '../services/notification/models/NotificationBasicInfoModel';

@Injectable({
  providedIn: 'root'
})
export class NotificationApiRepositoryService extends ApiRepository{

  constructor(http: HttpClient) {
    super(environments.smartHomeApi, http);
  }

  public getNotifications(
    tipoDispositivo: string, 
    fechaDeCreacion: string, 
    leida: string
  ):Observable<Array<NotificationBasicInfoModel>>{
    const query = `tipoDispositivo=${tipoDispositivo}&fechaDeCreacion=${fechaDeCreacion}&leida=${leida}`;
    return this.get<Array<NotificationBasicInfoModel>>('notificaciones', query);
  }

  public markAsRead(
    tipoDispositivo: string, 
    fechaDeCreacion: string, 
    leida: string
  ){
    return this.patch({}, `notificaciones/leidas?tipoDispositivo=${tipoDispositivo}&fechaDeCreacion=${fechaDeCreacion}&leida=${leida}`);
  }
}
