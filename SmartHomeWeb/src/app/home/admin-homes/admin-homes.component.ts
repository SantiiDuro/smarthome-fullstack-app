import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../../../backend/services/user/user.service';
import HomePermissionModel from '../../../backend/services/user/models/HomePermissionModel';

@Component({
  selector: 'app-admin-homes',
  templateUrl: './admin-homes.component.html',
  styles: ``
})
export class AdminHomesComponent {
  homePermissions: HomePermissionModel | null = null;

  private _selectedHome: string | null = null;

  homeName: string = "hogar";

  constructor(
    private readonly _router: Router,
    private readonly _userService: UserService
  ) {}

  get selectedHome(): string | null {
    return this._selectedHome;
  }

  set selectedHome(value: string | null) {
    this._selectedHome = value;
    this._router.navigate(['/home', 'myHomes']);
    if (value){
      this._userService.getHomePermissions(this.selectedHome || '').subscribe({
        next: (permissions) => {
          this.homePermissions = permissions;
        }
      });
    }
    else
    {
      this.homePermissions = null;
    }
  }

  navigateToAssignMember(){
    this._router.navigate(['/home', 'myHomes', this.selectedHome, 'assignMember']);
  }

  navigateToListHomeDevices(){
    this._router.navigate(['/home', 'myHomes', this.selectedHome, 'listDevices']);
  }

  navigateToListHomeMembers(){
    this._router.navigate(['/home', 'myHomes', this.selectedHome, 'listMembers']);
  }

  navigateToAssignDevice(){
    this._router.navigate(['/home', 'myHomes', this.selectedHome, 'assignDevice']);
  }

  navigateToModifyHomeName(){
    this._router.navigate(['/home', 'myHomes', this.selectedHome, 'modifyName']);
  }

  navigateToCreateRoom(){
    this._router.navigate(['/home', 'myHomes', this.selectedHome, 'createRoom']);
  }

  navigateToModifyHomeDeviceName(){
    this._router.navigate(['/home', 'myHomes', this.selectedHome, 'modifyDeviceName']);
  }

  navigateToConnectHomeDevice(){
    this._router.navigate(['/home', 'myHomes', this.selectedHome, 'connectDevice']);
  }

  navigateToDisconnectHomeDevice(){
    this._router.navigate(['/home', 'myHomes', this.selectedHome, 'disconnectDevice']);
  }

  navigateToAssignDeviceRoom(){
    this._router.navigate(['/home', 'myHomes', this.selectedHome, 'assignDeviceRoom']);
  }

  noPermissions(){
    return !this.homePermissions?.permisoAgregarMiembros &&
      !this.homePermissions?.permisoListarDispositivos &&
      !this.homePermissions?.permisoAsociarDispositivos &&
      !this.homePermissions?.permisoAdministrarCuartos &&
      !this.homePermissions?.permisoModificarNombreDispositivos &&
      !this.homePermissions?.permisoListarMiembros &&
      !this.homePermissions?.permisoModificarAlias;
  }
}
