import { NgFor, NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { SessionService } from '../../backend/services/session/session.service';
import { NavbarButtonComponent } from './navbar-button/navbar-button.component';
import { UserService } from '../../backend/services/user/user.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [NgFor, NavbarButtonComponent, NgIf],
  templateUrl: './navbar.component.html',
  styles: ``
})
export class NavbarComponent {
  constructor(
    private readonly _router: Router,
    private readonly _sessionService: SessionService,
    private readonly _userService: UserService
  ) { }

  title = 'SmartHome';

  buttons = [
    { title: 'Registrar Administrador', permission: 'CrearAdmin', action: () => this.onClickRegisterAdmin() },
    { title: 'Eliminar Administrador', permission: 'EliminarAdmin', action: () => this.onClickDeleteAdmin() },
    { title: 'Registrar Dueño Empresa', permission: 'CrearDueñoEmpresa', action: () => this.onClickRegisterBO() },
    { title: 'Listar Empresas', permission: 'ListarEmpresas', action: () => this.onClickGetCompanies() },
    { title: 'Listar Usuarios', permission: 'ListarUsuarios', action: () => this.onClickGetUsers() },
    { title: 'Crear Empresa', permission: 'CrearEmpresa', action: () => this.onClickCreateCompany() },
    { title: 'Crear Cámara', permission: 'CrearDispositivos', action: () => this.onClickCreateCamera() },
    { title: 'Crear Sensor', permission: 'CrearDispositivos', action: () => this.onClickCreateSensor() },
    { title: 'Crear Lámpara', permission: 'CrearDispositivos', action: () => this.onClickCreateLamp() },
    { title: 'Importar Dispositivos', permission: 'CrearDispositivos', action: () => this.onClickImportDevices() },
    { title: 'Crear Hogar', permission: 'CrearHogar', action: () => this.onClickCreateHome() },
    { title: 'Mis Hogares', permission: 'CrearHogar', action: () => this.onClickMyHomes() },
    { title: 'Notificaciones', permission: 'CrearHogar', action: () => this.onClickGetNotifications() },
    { title: 'Dispositivos Soportados', permission: '', action: () => this.onClickGetDevicesTypes() },
    { title: 'Listar Dispositivos', permission: '', action: () => this.onClickGetDevices() },
  ];

  lowerButtons = [
    { title: 'Añadir Permisos Hogar', permission: 'ActualizarRolUsuario', action: () => this.onClickUpdatePermissions() },
    { title: 'Cerrar Sesión', permission: '', action: () => this.onClickLogout() }
  ];

  hasPermission(permission: string): boolean {
    if (!permission) return true;
    const userPermissions = localStorage.getItem('permissions');
    return userPermissions ? JSON.parse(userPermissions).includes(permission) : false;  
  }

  onClickLogout() {
    this._sessionService.logout().subscribe({
      next: () => {
          this._router.navigate(['']);
      }
  });
  }

  onClickUpdatePermissions() {
    this._router.navigate(['/home', 'updatePermissions']);
  }

  onClickRegisterAdmin() {
    this._router.navigate(['/home', 'registerAdmin']);
  }

  onClickDeleteAdmin() {
    this._router.navigate(['/home', 'deleteAdmin']);
  }

  onClickRegisterBO() {
    this._router.navigate(['/home', 'registerBO']);
  }

  onClickGetCompanies() {
    this._router.navigate(['/home', 'companyList']);
  }

  onClickCreateCompany() {
    this._router.navigate(['/home', 'createCompany']);
  }

  onClickCreateCamera() {
    this._router.navigate(['/home', 'createCamera']);
  }

  onClickCreateSensor() {
    this._router.navigate(['/home', 'createSensor']);
  }
  
  onClickCreateLamp() {
    this._router.navigate(['/home', 'createLamp']);
  }

  onClickImportDevices() {
    this._router.navigate(['/home', 'importDevices']);
  }

  onClickGetDevicesTypes() {
    this._router.navigate(['/home', 'devicesTypes']);
  }

  onClickGetDevices() {
    this._router.navigate(['/home', 'deviceList']);
  }

  onClickGetUsers() {
    this._router.navigate(['/home', 'userList']);
  }

  onClickCreateHome() {
    this._router.navigate(['/home', 'createHome']);
  }

  onClickMyHomes() {
    this._router.navigate(['/home', 'myHomes']);
  }

  onClickGetNotifications() {
    this._router.navigate(['/home', 'notifications']);
  }
}
