import { Injectable } from '@angular/core';
import { HomeApiRepositoryService } from '../../repositories/home-api-repository.service';
import HomeCreateModel from './models/HomeCreateModel';
import { Observable } from 'rxjs';
import HomeBasicInfoModel from './models/HomeBasicInfoModel';
import AssignMemberModel from './models/AssignMemberModel';
import MemberBasicInfoModel from './models/MemberBasicInfoModel';
import HomeDeviceBasicInfoModel from './models/HomeDeviceBasicInfoModel';
import ModifyNameModel from './models/ModifyNameModel';
import CreateRoomModel from './models/CreateRoomModel';
import HomeRoomBasicInfoModel from './models/HomeRoomBasicInfoModel';
import AssignDeviceModel from './models/AssignDeviceModel';

@Injectable({
  providedIn: 'root'
})
export class HomeService {

  constructor(private readonly _repository: HomeApiRepositoryService) { }

  public createHome(
    credentials: HomeCreateModel
  ) {
    return this._repository.createHome(credentials);
  }

  public getHomesFromUser(): Observable<Array<HomeBasicInfoModel>> {
    return this._repository.getHomesFromUser();
  }

  public assignMember(
    homeId: string,
    credentials: AssignMemberModel
  ) {
    return this._repository.assignMember(homeId, credentials);
  }

  public getMembers(
    homeId: string,
  ): Observable<Array<MemberBasicInfoModel>> {
    return this._repository.getMembers(homeId);
  }

  public getHomeDevices(
    nombreCuarto = '',
    homeId: string,
  ): Observable<Array<HomeDeviceBasicInfoModel>> {
    return this._repository.getHomeDevices(nombreCuarto, homeId);
  }

  public modifyName(
    homeId: string,
    credentials: ModifyNameModel
  ) {
    return this._repository.modifyName(homeId, credentials);
  }

  public createRoom(
    homeId: string,
    credentials: CreateRoomModel
  ) {
    return this._repository.createRoom(homeId, credentials);
  }

  public getHomeRooms(
    homeId: string,
  ): Observable<Array<HomeRoomBasicInfoModel>> {
    return this._repository.getHomeRooms(homeId);
  }

  public addDevice(
    homeId: string,
    credentials: AssignDeviceModel
  ) {
    return this._repository.addDevice(homeId, credentials);
  }
}
