import { Injectable } from '@angular/core';
import { CompanyApiRepositoryService } from '../../repositories/company-api-repository.service';
import { Observable } from 'rxjs';
import CompaniesResponseModel from './models/CompaniesResponseModel';
import ValidatorBasicInfoModel from './models/ValidatorBasicInfoModel';
import CreateCompanyModel from './models/CreateCompanyModel';

@Injectable({
  providedIn: 'root'
})
export class CompanyService {

  constructor(private readonly _repository: CompanyApiRepositoryService) { }

  public getCompanies(
    pageNumber: number,
    pageSize: number,
    companyName = '',
    ownerName = ''): Observable<CompaniesResponseModel> {
    return this._repository.getCompanies(pageNumber, pageSize, companyName, ownerName);
  }

  public getValidators(): Observable<Array<ValidatorBasicInfoModel>>{
    return this._repository.getValidators();
  }

  public createCompany(
    credentials: CreateCompanyModel
  ) {
    return this._repository.createCompany(credentials);
  }
} 
