import { Injectable } from '@angular/core';
import ModifyHomeDeviceNameModel from './models/ModifyHomeDeviceNameModel';
import { HomeDeviceApiRepositoryService } from '../../repositories/home-device-api-repository.service';

@Injectable({
  providedIn: 'root'
})
export class HomeDeviceService {

  constructor(private readonly _repository: HomeDeviceApiRepositoryService) { }

  public modifyName(
    credentials: ModifyHomeDeviceNameModel,
    homeDeviceId: string
  ){
    return this._repository.modifyName(credentials, homeDeviceId);
  }

  public connect(homeDeviceId: string){
    return this._repository.connect(homeDeviceId);
  }

  public disconnect(homeDeviceId: string){
    return this._repository.disconnect(homeDeviceId);
  }
}
