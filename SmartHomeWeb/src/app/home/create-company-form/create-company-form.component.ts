import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { CompanyService } from '../../../backend/services/company/company.service';
import { Router } from '@angular/router';
import CreateCompanyModel from '../../../backend/services/company/models/CreateCompanyModel';

@Component({
  selector: 'app-create-company-form',
  templateUrl: './create-company-form.component.html',
  styles: ``
})
export class CreateCompanyFormComponent {
  selectedValidator: string | null = null;

  readonly formField: any = {
    nombre: {
      name: "nombre",
      required: "Nombre es requerido",
    },
    rut: {
      name: "rut",
      required: "Rut es requerido",
    },
    logotipo: {
      name: "logotipo",
      required: "Logotipo es requerido",
    },
    validador: {
      name: "validador",
      required: "Validador es requerido",
    },
  };

  readonly createCompanyForm = new FormGroup({
    [this.formField.nombre.name]: new FormControl("", [
      Validators.required
    ]),
    [this.formField.rut.name]: new FormControl("", [
      Validators.required
    ]),
    [this.formField.logotipo.name]: new FormControl("", [
      Validators.required,
    ]),
    [this.formField.validador.name]: new FormControl("", [
      Validators.required,
    ])
  });

  createStatus: {
    loading?: true;
    error?: string;
  } | null = null;

  constructor(
    private readonly _router: Router,
    private readonly _companyService: CompanyService
  ) { }

  public onSubmit(values: CreateCompanyModel){
    this.createStatus = { loading: true };
    
    this._companyService.createCompany(values).subscribe({
      next: (response) => {
        this.createStatus = null;

        this._router.navigate(["/home"]);
      },
      error: (error) => {
        this.createStatus = { error };
      }
    });
  }
}
