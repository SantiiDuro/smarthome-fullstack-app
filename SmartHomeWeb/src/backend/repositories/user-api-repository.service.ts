import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import environments from '../../environments';
import ApiRepository from './api-repository';
import HomeOwnerRegisterModel from '../services/user/models/HomeOwnerRegisterModel';
import AdminRegisterModel from '../services/user/models/AdminRegisterModel';
import BORegisterModel from '../services/user/models/BORegisterModel';
import AdminDeleteModel from '../services/user/models/AdminDeleteModel';
import { Observable } from 'rxjs';
import UsersResponseModel from '../services/user/models/UsersResponseModel';
import UpdatePermissionsModel from '../services/user/models/UpdatePermissionsModel';
import HomePermissionModel from '../services/user/models/HomePermissionModel';
import UpdateProfileModel from '../services/user/models/UpdateProfileModel';

@Injectable({
  providedIn: 'root'
})
export class UserApiRepositoryService extends ApiRepository {

  constructor(http: HttpClient) {
    super(environments.smartHomeApi, http);
  }

  public registerHomeOwner(
    credentials: HomeOwnerRegisterModel
  ){
    return this.post(credentials, "dueños-hogar");
  }

  public registerAdmin(
    credentials: AdminRegisterModel
  ){
    return this.post(credentials, "administradores");
  }

  public registerBO(
    credentials: BORegisterModel
  ){
    return this.post(credentials, "dueños-empresa");
  }

  public deleteAdmin(
    credentials: AdminDeleteModel
  ){
    return this.delete(`administradores/${credentials.email}`);
  }

  public getUsers(
    page: number,
    pageSize: number,
    rol: string,
    nombreCompleto: string
  ): Observable<UsersResponseModel>{
    const query = `numeroDePagina=${page}&tamañoDePagina=${pageSize}&rol=${rol}&nombreCompleto=${nombreCompleto}`;
    return this.get<UsersResponseModel>("usuarios", query);
  }

  public updateProfile(
    credentials: UpdateProfileModel
  ){
    return this.patch(credentials, "usuarios/foto-perfil");
  }

  public updateUserPermissions(): Observable<UpdatePermissionsModel>{
    return this.patch<UpdatePermissionsModel>({}, "usuarios/permisos");
  }

  public getHomePermissions(homeId: string): Observable<HomePermissionModel>{
    return this.get<HomePermissionModel>(`dueños-hogar/${homeId}/permisos`);
  }
}
