import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomePageComponent } from './home-page/home-page.component';
import { CreateAdminFormComponent } from './create-admin-form/create-admin-form.component';
import { authGuard } from '../../guards/auth.guard';
import { CreateBOFormComponent } from './create-bo-form/create-bo-form.component';
import { CompaniesComponent } from './companies/companies.component';
import { DevicesTypesComponent } from './devices-types/devices-types.component';
import { DeleteAdminFormComponent } from './delete-admin-form/delete-admin-form.component';
import { DevicesComponent } from './devices/devices.component';
import { UsersComponent } from './users/users.component';
import { CreateCompanyFormComponent } from './create-company-form/create-company-form.component';
import { CreateCameraFormComponent } from './create-camera-form/create-camera-form.component';
import { CreateSensorFormComponent } from './create-sensor-form/create-sensor-form.component';
import { CreateLampFormComponent } from './create-lamp-form/create-lamp-form.component';
import { CreateHomeFormComponent } from './create-home-form/create-home-form.component';
import { AssignMemberFormComponent } from './assign-member-form/assign-member-form.component';
import { WelcomeComponent } from './welcome/welcome.component';
import { AdminHomesComponent } from './admin-homes/admin-homes.component';
import { ShowMembersComponent } from './show-members/show-members.component';
import { ShowHomeDevicesComponent } from './show-home-devices/show-home-devices.component';
import { ModifyHomeNameFormComponent } from './modify-home-name-form/modify-home-name-form.component';
import { CreateRoomFormComponent } from './create-room-form/create-room-form.component';
import { ModifyHomeDeviceNameFormComponent } from './modify-home-device-name-form/modify-home-device-name-form.component';
import { ConnectHomeDeviceFormComponent } from './connect-home-device-form/connect-home-device-form.component';
import { DisconnectHomeDeviceFormComponent } from './disconnect-home-device-form/disconnect-home-device-form.component';
import { AssignDeviceRoomFormComponent } from './assign-device-room-form/assign-device-room-form.component';
import { AssignDeviceHomeFormComponent } from './assign-device-home-form/assign-device-home-form.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { ImportDevicesFormComponent } from './import-devices-form/import-devices-form.component';
import { UpdatePermissionsFormComponent } from './update-permissions-form/update-permissions-form.component';

const routes: Routes = [
  {
    path: '',
    component: HomePageComponent,
    children: [
      {
        path: 'welcome',
        component: WelcomeComponent
      },
      {
        path: 'registerAdmin',
        canActivate: [authGuard],
        data: { requiredPermission: 'CrearAdmin' },
        component: CreateAdminFormComponent,
      },
      {
        path: 'deleteAdmin',
        canActivate: [authGuard],
        data: { requiredPermission: 'EliminarAdmin' },
        component: DeleteAdminFormComponent,
      },
      {
        path: 'registerBO',
        canActivate: [authGuard],
        data: { requiredPermission: 'CrearDueñoEmpresa' },
        component: CreateBOFormComponent,
      },
      {
        path: 'companyList',
        canActivate: [authGuard],
        data: { requiredPermission: 'ListarEmpresas' },
        component: CompaniesComponent,
      },
      {
        path: 'devicesTypes',
        canActivate: [authGuard],
        component: DevicesTypesComponent,
      },
      {
        path: 'deviceList',
        canActivate: [authGuard],
        component: DevicesComponent,
      },
      {
        path: 'userList',
        canActivate: [authGuard],
        data: { requiredPermission: 'ListarUsuarios' },
        component: UsersComponent,
      },
      {
        path: 'createCompany',
        canActivate: [authGuard],
        data: { requiredPermission: 'CrearEmpresa' },
        component: CreateCompanyFormComponent,
      },
      {
        path: 'createCamera',
        canActivate: [authGuard],
        data: { requiredPermission: 'CrearDispositivos' },
        component: CreateCameraFormComponent,
      },
      {
        path: 'createSensor',
        canActivate: [authGuard],
        data: { requiredPermission: 'CrearDispositivos' },
        component: CreateSensorFormComponent,
      },
      {
        path: 'createLamp',
        canActivate: [authGuard],
        data: { requiredPermission: 'CrearDispositivos' },
        component: CreateLampFormComponent,
      },
      {
        path: 'importDevices',
        canActivate: [authGuard],
        data: { requiredPermission: 'CrearDispositivos' },
        component: ImportDevicesFormComponent,
      },
      {
        path: 'createHome',
        canActivate: [authGuard],
        data: { requiredPermission: 'CrearHogar' },
        component: CreateHomeFormComponent,
      },
      {
        path: 'myHomes',
        canActivate: [authGuard],
        data: { requiredPermission: 'CrearHogar' },
        component: AdminHomesComponent,
        children: [
          {
            path: ':homeId/assignMember',
            component: AssignMemberFormComponent
          },
          {
            path: ':homeId/listMembers',
            component: ShowMembersComponent
          },
          {
            path: ':homeId/assignDevice',
            component: AssignDeviceHomeFormComponent
          },
          {
            path: ':homeId/listDevices',
            component: ShowHomeDevicesComponent
          },
          {
            path: ':homeId/modifyName',
            component: ModifyHomeNameFormComponent
          },
          {
            path: ':homeId/createRoom',
            component: CreateRoomFormComponent
          },
          {
            path: ':homeId/modifyDeviceName',
            component: ModifyHomeDeviceNameFormComponent
          },
          {
            path: ':homeId/connectDevice',
            component: ConnectHomeDeviceFormComponent
          },
          {
            path: ':homeId/disconnectDevice',
            component: DisconnectHomeDeviceFormComponent
          },
          {
            path: ':homeId/assignDeviceRoom',
            component: AssignDeviceRoomFormComponent
          },
        ]
      },
      {
        path: 'notifications',
        canActivate: [authGuard],
        data: { requiredPermission: 'CrearHogar' },
        component: NotificationsComponent
      },
      {
        path: 'updatePermissions',
        canActivate: [authGuard],
        data: { requiredPermission: 'ActualizarRolUsuario' },
        component: UpdatePermissionsFormComponent
      },
      {
        path: '',
        redirectTo: 'welcome',
        pathMatch: 'full',
      },
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HomeRoutingModule { }
