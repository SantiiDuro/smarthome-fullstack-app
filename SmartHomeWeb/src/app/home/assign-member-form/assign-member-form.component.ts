import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HomeService } from '../../../backend/services/home/home.service';
import AssignMemberModel from '../../../backend/services/home/models/AssignMemberModel';
import HomePermissionModel from '../../../backend/services/user/models/HomePermissionModel';
import { UserService } from '../../../backend/services/user/user.service';

@Component({
  selector: 'app-assign-member-form',
  templateUrl: './assign-member-form.component.html',
  styles: ``
})
export class AssignMemberFormComponent {
  selectedHome: string | null = null;

  readonly formField: any = {
    email: {
      name: "email",
      required: "Email es requerido",
    },
  };

  readonly asignMemberForm = new FormGroup({
    [this.formField.email.name]: new FormControl("", [
      Validators.required,
      Validators.email,
    ]),
  });

  checkboxOptions = {
    permisoAsociarDispositivos: false,
    permisoListarDispositivos: false,
    permisoNotificaciones: false,
    perimsoAdministrarCuartos: false,
    permisoModificarNombreDispositivos: false,
  };

  asignStatus: {
    loading?: true;
    error?: string;
  } | null = null;

  constructor(
    private readonly _router: Router,
    private readonly _route: ActivatedRoute,
    private readonly _homeService: HomeService,
  ) { }

  ngOnInit(): void {
    this._route.params.subscribe((params) => {
      this.selectedHome = params['homeId'] || null;

      if (!this.selectedHome) {
        this._router.navigate(['/home', 'myHomes']);
      }
    });
  }

  onCheckboxOptionsChange(options: { 
    permisoAsociarDispositivos: boolean; 
    permisoListarDispositivos: boolean; 
    permisoNotificaciones: boolean; 
    perimsoAdministrarCuartos: boolean; 
    permisoModificarNombreDispositivos: boolean;
  }) {
    this.checkboxOptions = options;
  }

  public onSubmit(){
    this.asignStatus = { loading: true };

    const formValue = this.asignMemberForm.value;
    const model: AssignMemberModel = {
      email: formValue[this.formField.email.name] || '',
      permisoAsociarDispositivos: this.checkboxOptions.permisoAsociarDispositivos,
      permisoListarDispositivos: this.checkboxOptions.permisoListarDispositivos,
      permisoNotificaciones: this.checkboxOptions.permisoNotificaciones,
      perimsoAdministrarCuartos: this.checkboxOptions.perimsoAdministrarCuartos,
      permisoModificarNombreDispositivos: this.checkboxOptions.permisoModificarNombreDispositivos,
    };

    this._homeService.assignMember(this.selectedHome || '', model).subscribe({
      next: (response) => {
        this._router.navigate(['/home', 'myHomes']);
      },
      error: (error) => {
        this.asignStatus = { error };
      }
    });
  }
}
