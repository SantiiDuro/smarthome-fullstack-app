import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../../../backend/services/user/user.service';
import { Router } from '@angular/router';
import UpdateProfileModel from '../../../backend/services/user/models/UpdateProfileModel';

@Component({
  selector: 'app-update-permissions-form',
  templateUrl: './update-permissions-form.component.html',
  styles: ``
})
export class UpdatePermissionsFormComponent {
  readonly formField: any = {
    fotoPerfil: {
      name: "fotoPerfil",
      required: "Foto de perfil es requerida",
    }
  };

  readonly updatePermissionsForm = new FormGroup({
    [this.formField.fotoPerfil.name]: new FormControl("", [
      Validators.required
    ])
  });

  updateStatus: {
    loading?: true;
    error?: string;
  } | null = null;

  constructor(
    private readonly _router: Router,
    private readonly _userService: UserService
  ) { }

  public onSubmit(values: UpdateProfileModel) {
    this.updateStatus = { loading: true };

    this._userService.updateProfile(values).subscribe({
      next: (response) => {
        this.updateStatus = null;

        this._userService.updateUserPermissions().subscribe();

        this._router.navigate(["/home"]);
      },
      error: (error) => {
        this.updateStatus = { error };
      },
    });
  }
}
