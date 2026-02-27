import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { HomeRoomService } from '../../../backend/services/home-room/home-room.service';
import AssignDeviceRoomModel from '../../../backend/services/home-room/models/AssignDeviceRoomModel';

@Component({
  selector: 'app-assign-device-room-form',
  templateUrl: './assign-device-room-form.component.html',
  styles: ``
})
export class AssignDeviceRoomFormComponent {
  selectedHomeDevice: string | null = null;
  selectedHomeRoom: string | null = null;

  readonly formField: any = {
    homeDevice: {
      name: "homeDevice",
      required: "Dispositivo es requerido",
    },
    homeRoom: {
      name: "homeRoom",
      required: "Cuarto es requerido",
    },
  };

  readonly assignHomeRoomForm = new FormGroup({
    [this.formField.homeDevice.name]: new FormControl("", [
      Validators.required,
    ]),
    [this.formField.homeRoom.name]: new FormControl("", [
      Validators.required,
    ]),
  });

  assignStatus: {
    loading?: true;
    error?: string;
  } | null = null;

  constructor(
    private readonly _router: Router,
    private readonly _homeRoomService: HomeRoomService
  ) { }

  public onSubmit(){
    this.assignStatus = { loading: true };
    
    const model: AssignDeviceRoomModel = {
      dispositivoHogarId: this.selectedHomeDevice || '',
    }

    this._homeRoomService.addDevice(this.selectedHomeRoom || '', model).subscribe({
      next: (response) => {
        this.assignStatus = null;

        this._router.navigate(['/home', 'myHomes']);
      },
      error: (error) => {
        this.assignStatus = { error };
      }
    });
  }
}
