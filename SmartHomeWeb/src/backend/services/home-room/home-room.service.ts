import { Injectable } from '@angular/core';
import AssignDeviceRoomModel from './models/AssignDeviceRoomModel';
import { HomeRoomApiRepositoryService } from '../../repositories/home-room-api-repository.service';

@Injectable({
  providedIn: 'root'
})
export class HomeRoomService {

  constructor(private readonly _repository: HomeRoomApiRepositoryService) { }

  public addDevice(
    roomId: string,
    credentials: AssignDeviceRoomModel
  ){
    return this._repository.addDevice(roomId, credentials);
  }
}
