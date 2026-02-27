import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HomeRoutingModule } from './home-routing.module';
import { HomePageComponent } from './home-page/home-page.component';
import { NavbarComponent } from "../../components/navbar/navbar.component";
import { CreateAdminFormComponent } from './create-admin-form/create-admin-form.component';
import { FormComponent } from '../../components/form-elements/form/form.component';
import { FormInputComponent } from '../../components/form-elements/form-input/form-input.component';
import { FormButtonComponent } from '../../components/form-elements/form-button/form-button.component';
import { RouterModule } from '@angular/router';
import { CreateBOFormComponent } from './create-bo-form/create-bo-form.component';
import { CompaniesComponent } from './companies/companies.component';
import { CompanyTableComponent } from '../../business-components/company-table/company-table.component';
import { DevicesTypesComponent } from './devices-types/devices-types.component';
import { DeviceTypeListComponent } from '../../business-components/device-type-list/device-type-list.component';
import { DeleteAdminFormComponent } from './delete-admin-form/delete-admin-form.component';
import { NavbarButtonComponent } from '../../components/navbar/navbar-button/navbar-button.component';
import { DevicesComponent } from './devices/devices.component';
import { DeviceTableComponent } from '../../business-components/device-table/device-table.component';
import { UsersComponent } from './users/users.component';
import { UserTableComponent } from '../../business-components/user-table/user-table.component';
import { CreateCompanyFormComponent } from './create-company-form/create-company-form.component';
import { ValidatorsDropdownComponent } from '../../business-components/validators-dropdown/validators-dropdown.component';
import { CreateCameraFormComponent } from './create-camera-form/create-camera-form.component';
import { SecondaryImagesComponent } from "../../business-components/secondary-images/secondary-images.component";
import { CameraCheckboxComponent } from "../../business-components/camera-checkbox/camera-checkbox.component";
import { CreateSensorFormComponent } from './create-sensor-form/create-sensor-form.component';
import { SensorTypeDropdownComponent } from '../../business-components/sensor-type-dropdown/sensor-type-dropdown.component';
import { CreateLampFormComponent } from './create-lamp-form/create-lamp-form.component';
import { CreateHomeFormComponent } from './create-home-form/create-home-form.component';
import { AssignMemberFormComponent } from './assign-member-form/assign-member-form.component';
import { UserHomesDropdownComponent } from "../../business-components/user-homes-dropdown/user-homes-dropdown.component";
import { HomePermissionsCheckboxComponent } from "../../business-components/home-permissions-checkbox/home-permissions-checkbox.component";
import { WelcomeComponent } from './welcome/welcome.component';
import { AdminHomesComponent } from './admin-homes/admin-homes.component';
import { ButtonComponent } from '../../components/button/button.component';
import { ShowMembersComponent } from './show-members/show-members.component';
import { MembersTableComponent } from '../../business-components/members-table/members-table.component';
import { ShowHomeDevicesComponent } from './show-home-devices/show-home-devices.component';
import { HomeDevicesTableComponent } from '../../business-components/home-devices-table/home-devices-table.component';
import { ModifyHomeNameFormComponent } from './modify-home-name-form/modify-home-name-form.component';
import { CreateRoomFormComponent } from './create-room-form/create-room-form.component';
import { ModifyHomeDeviceNameFormComponent } from './modify-home-device-name-form/modify-home-device-name-form.component';
import { HomeDevicesDropdownComponent } from "../../business-components/home-devices-dropdown/home-devices-dropdown.component";
import { ConnectHomeDeviceFormComponent } from './connect-home-device-form/connect-home-device-form.component';
import { DisconnectHomeDeviceFormComponent } from './disconnect-home-device-form/disconnect-home-device-form.component';
import { AssignDeviceRoomFormComponent } from './assign-device-room-form/assign-device-room-form.component';
import { HomeRoomsDropdownComponent } from '../../business-components/home-rooms-dropdown/home-rooms-dropdown.component';
import { AssignDeviceHomeFormComponent } from './assign-device-home-form/assign-device-home-form.component';
import { DeviceDropdownComponent } from "../../business-components/device-dropdown/device-dropdown.component";
import { NotificationsComponent } from './notifications/notifications.component';
import { NotificationTableComponent } from '../../business-components/notification-table/notification-table.component';
import { ImportDevicesFormComponent } from './import-devices-form/import-devices-form.component';
import { ImportersDropdownComponent } from '../../business-components/importers-dropdown/importers-dropdown.component';
import { UpdatePermissionsFormComponent } from './update-permissions-form/update-permissions-form.component';


@NgModule({
  declarations: [
    HomePageComponent,
    CreateAdminFormComponent,
    CreateBOFormComponent,
    CompaniesComponent,
    DevicesTypesComponent,
    DeleteAdminFormComponent,
    DevicesComponent,
    UsersComponent,
    CreateCompanyFormComponent,
    CreateCameraFormComponent,
    CreateSensorFormComponent,
    CreateLampFormComponent,
    CreateHomeFormComponent,
    AssignMemberFormComponent,
    WelcomeComponent,
    AdminHomesComponent,
    ShowMembersComponent,
    ShowHomeDevicesComponent,
    ModifyHomeNameFormComponent,
    CreateRoomFormComponent,
    ModifyHomeDeviceNameFormComponent,
    ConnectHomeDeviceFormComponent,
    DisconnectHomeDeviceFormComponent,
    AssignDeviceRoomFormComponent,
    AssignDeviceHomeFormComponent,
    NotificationsComponent,
    ImportDevicesFormComponent,
    UpdatePermissionsFormComponent
  ],
  imports: [
    CommonModule,
    HomeRoutingModule,
    NavbarComponent,
    FormComponent,
    FormInputComponent,
    FormButtonComponent,
    RouterModule,
    DeviceTypeListComponent,
    CompanyTableComponent,
    NavbarButtonComponent,
    DeviceTableComponent,
    UserTableComponent,
    ValidatorsDropdownComponent,
    SecondaryImagesComponent,
    CameraCheckboxComponent,
    SensorTypeDropdownComponent,
    UserHomesDropdownComponent,
    HomePermissionsCheckboxComponent,
    ButtonComponent,
    MembersTableComponent,
    HomeDevicesTableComponent,
    HomeDevicesDropdownComponent,
    HomeRoomsDropdownComponent,
    DeviceDropdownComponent,
    NotificationTableComponent,
    ImportersDropdownComponent
]
})
export class HomeModule { }
