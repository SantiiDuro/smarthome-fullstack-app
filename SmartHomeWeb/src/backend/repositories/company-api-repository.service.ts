import { Injectable } from '@angular/core';
import ApiRepository from './api-repository';
import { HttpClient } from '@angular/common/http';
import environments from '../../environments';
import { Observable } from 'rxjs';
import CompaniesResponseModel from '../services/company/models/CompaniesResponseModel';
import ValidatorBasicInfoModel from '../services/company/models/ValidatorBasicInfoModel';
import CreateCompanyModel from '../services/company/models/CreateCompanyModel';

@Injectable({
  providedIn: 'root'
})
export class CompanyApiRepositoryService extends ApiRepository {

  constructor(http: HttpClient) {
    super(environments.smartHomeApi, http);
  }

  public getCompanies(
    pageNumber: number,
    pageSize: number,
    companyName: string,
    ownerName: string): Observable<CompaniesResponseModel> {
    const query = `numeroDePagina=${pageNumber}&tamañoDePagina=${pageSize}&nombre=${companyName}&nombreCompletoCreador=${ownerName}`;
    return this.get<CompaniesResponseModel>("empresas", query);
  }

  public getValidators(): Observable<Array<ValidatorBasicInfoModel>> {
    return this.get<Array<ValidatorBasicInfoModel>>("empresas/validadores");
  }

  public createCompany(
    credentials: CreateCompanyModel
  ) {
    return this.post(credentials, "empresas");
  }
}
