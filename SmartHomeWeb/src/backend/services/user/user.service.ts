import { Injectable } from '@angular/core';
import { UserApiRepositoryService } from '../../repositories/user-api-repository.service';
import HomeOwnerRegisterModel from './models/HomeOwnerRegisterModel';
import AdminRegisterModel from './models/AdminRegisterModel';
import BORegisterModel from './models/BORegisterModel';
import AdminDeleteModel from './models/AdminDeleteModel';
import { Observable, tap } from 'rxjs';
import UsersResponseModel from './models/UsersResponseModel';
import UpdatePermissionsModel from './models/UpdatePermissionsModel';
import HomePermissionModel from './models/HomePermissionModel';
import UpdateProfileModel from './models/UpdateProfileModel';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private readonly _repository: UserApiRepositoryService) { }

  public registerHomeOwner(
    credentials: HomeOwnerRegisterModel
  ){
    return this._repository.registerHomeOwner(credentials);
  }

  public registerAdmin(
    credentials: AdminRegisterModel
  ){
    return this._repository.registerAdmin(credentials);
  }

  public registerBO(
    credentials: BORegisterModel
  ){
    return this._repository.registerBO(credentials);
  }

  public deleteAdmin(
    credentials: AdminDeleteModel
  ){
    return this._repository.deleteAdmin(credentials);
  }

  public getUsers(
    page: number,
    pageSize: number,
    rol = '',
    nombreCompleto = ''
  ): Observable<UsersResponseModel>{
    return this._repository.getUsers(page, pageSize, rol, nombreCompleto);
  }

  public updateProfile(
    credentials: UpdateProfileModel
  ){
    return this._repository.updateProfile(credentials);
  }

  public updateUserPermissions(): Observable<UpdatePermissionsModel>{
    return this._repository.updateUserPermissions().pipe(
      tap((permissions) => {
        localStorage.setItem('permissions', JSON.stringify(permissions.permisos));
      })
    );
  }

  public getHomePermissions(homeId: string): Observable<HomePermissionModel>{
    return this._repository.getHomePermissions(homeId);
  }
}
