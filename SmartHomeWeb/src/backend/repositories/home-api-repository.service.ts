import { Injectable } from '@angular/core';
import ApiRepository from './api-repository';
import { HttpClient } from '@angular/common/http';
import environments from '../../environments';
import HomeCreateModel from '../services/home/models/HomeCreateModel';
import { Observable } from 'rxjs';
import HomeBasicInfoModel from '../services/home/models/HomeBasicInfoModel';
import AssignMemberModel from '../services/home/models/AssignMemberModel';
import MemberBasicInfoModel from '../services/home/models/MemberBasicInfoModel';
import HomeDeviceBasicInfoModel from '../services/home/models/HomeDeviceBasicInfoModel';
import ModifyNameModel from '../services/home/models/ModifyNameModel';
import CreateRoomModel from '../services/home/models/CreateRoomModel';
import HomeRoomBasicInfoModel from '../services/home/models/HomeRoomBasicInfoModel';
import AssignDeviceModel from '../services/home/models/AssignDeviceModel';

@Injectable({
  providedIn: 'root'
})
export class HomeApiRepositoryService extends ApiRepository{

  constructor(http: HttpClient) {
    super(environments.smartHomeApi, http);
  }

  public createHome(
    credentials: HomeCreateModel
  ) {
    return this.post(credentials, 'hogares');
  }

  public getHomesFromUser(): Observable<Array<HomeBasicInfoModel>> {
    return this.get('hogares/usuario');
  }

  public assignMember(
    homeId: string,
    credentials: AssignMemberModel
  ) {
    return this.post(credentials, `hogares/${homeId}/miembros`);
  }

  public getMembers(
    homeId: string,
  ): Observable<Array<MemberBasicInfoModel>> {
    return this.get<Array<MemberBasicInfoModel>>(`hogares/${homeId}/miembros`);
  }

  public getHomeDevices(
    nombreCuarto: string,
    homeId: string,
  ): Observable<Array<HomeDeviceBasicInfoModel>> {
    const query = `nombreCuarto=${nombreCuarto}`;
    return this.get<Array<HomeDeviceBasicInfoModel>>(`hogares/${homeId}/dispositivos`, query);
  }

  public modifyName(
    homeId: string,
    credentials: ModifyNameModel
  ) {
    return this.patch(credentials, `hogares/${homeId}/alias`);
  }

  public createRoom(
    homeId: string,
    credentials: CreateRoomModel
  ) {
    return this.post(credentials, `hogares/${homeId}/cuartos`);
  }

  public getHomeRooms(
    homeId: string,
  ): Observable<Array<HomeRoomBasicInfoModel>> {
    return this.get<Array<HomeRoomBasicInfoModel>>(`hogares/${homeId}/cuartos`);
  }

  public addDevice(
    homeId: string,
    credentials: AssignDeviceModel
  ) {
    return this.post(credentials, `hogares/${homeId}/dispositivos`);
  }
}
