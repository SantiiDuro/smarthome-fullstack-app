import { Component } from '@angular/core';
import { UserService } from '../../../backend/services/user/user.service';
import { Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import AdminDeleteModel from '../../../backend/services/user/models/AdminDeleteModel';

@Component({
  selector: 'app-delete-admin-form',
  templateUrl: './delete-admin-form.component.html',
  styles: ``
})
export class DeleteAdminFormComponent {
  readonly formField: any = {
    email: {
      name: "email",
      required: "Email es requerido",
      email: "Email no es valido",
    }
  };

  readonly deleteAdminForm = new FormGroup({
    [this.formField.email.name]: new FormControl("", [
      Validators.required,
      Validators.email,
    ])
  });

  deleteStatus: {
    loading?: true;
    error?: string;
  } | null = null;

  constructor(
    private readonly _router: Router,
    private readonly _userService: UserService
  ) { }

  public onSubmit(values: AdminDeleteModel) {
    this.deleteStatus = { loading: true };

    this._userService.deleteAdmin(values).subscribe({
      next: (response) => {
        this.deleteStatus = null;

        this._router.navigate(["/home"]);
      },
      error: (error) => {
        this.deleteStatus = { error };
      },
    });
  }
}
