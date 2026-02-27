import { Component } from '@angular/core';
import { CompanyService } from '../../backend/services/company/company.service';
import { FormControl, FormGroup } from '@angular/forms';
import CompanyBasicInfoModel from '../../backend/services/company/models/CompanyBasicInfoModel';
import { TableComponent } from '../../components/table/table.component';
import { CommonModule, NgFor } from '@angular/common';
import { FormComponent } from '../../components/form-elements/form/form.component';
import { FormInputComponent } from '../../components/form-elements/form-input/form-input.component';
import { FormButtonComponent } from '../../components/form-elements/form-button/form-button.component';

@Component({
  selector: 'app-company-table',
  standalone: true,
  imports: [TableComponent, CommonModule, FormComponent, FormInputComponent, FormButtonComponent],
  templateUrl: './company-table.component.html',
  styles: ``
})
export class CompanyTableComponent {
  companies: CompanyBasicInfoModel[] = [];
  currentPage = 1;
  pageSize = 10;
  totalPages = 0;

  readonly formField: any = {
    nombreEmpresa: {
      name: "nombreEmpresa"
    },
    nombreCompletoDueño: {
      name: "nombreCompletoDueño"
    },
  };

  get nombreCompletoDuenoField() {
    return this.formField.nombreCompletoDueño;
  }

  readonly filterForm = new FormGroup({
    [this.formField.nombreEmpresa.name]: new FormControl("", []),
    [this.formField.nombreCompletoDueño.name]: new FormControl("", []),
  });

  constructor(
    private readonly companyService: CompanyService,
  ) {}

  getOwnerName(company: any): string {
    return company.nombreDueño;
  }

  ngOnInit() {
    this.loadCompanies();
  }

  loadCompanies() {
    const nombreEmpresa = this.filterForm.get('nombreEmpresa')?.value;
    const nombreCompletoDueño = this.filterForm.get('nombreCompletoDueño')?.value;

    this.companyService.getCompanies(this.currentPage, this.pageSize, nombreEmpresa, nombreCompletoDueño).subscribe({
      next: (response) => {
        this.companies = response.empresas;
        this.totalPages = response.cantidadPaginas;
      },
      error: (error) => {
        console.error('Error loading companies', error);
      }
    });
  }

  onFilterSubmit() {
    this.currentPage = 1;
    this.loadCompanies();
  }

  goToPreviousPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadCompanies();
    }
  }

  goToNextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadCompanies();
    }
  }
}
