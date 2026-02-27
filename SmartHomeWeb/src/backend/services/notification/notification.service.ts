import { Injectable } from '@angular/core';
import { NotificationApiRepositoryService } from '../../repositories/notification-api-repository.service';
import { Observable } from 'rxjs';
import NotificationBasicInfoModel from './models/NotificationBasicInfoModel';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor(private readonly _repository: NotificationApiRepositoryService) { }

  public getNotifications(
    tipoDispositivo = "",
    fechaDeCreacion = "",
    leida = ""
  ):Observable<Array<NotificationBasicInfoModel>>{
    return this._repository.getNotifications(tipoDispositivo, fechaDeCreacion, leida);
  }

  public markAsRead(
    tipoDispositivo = "",
    fechaDeCreacion = "",
    leida = ""
  ){
    return this._repository.markAsRead(tipoDispositivo, fechaDeCreacion, leida);
  }
}
