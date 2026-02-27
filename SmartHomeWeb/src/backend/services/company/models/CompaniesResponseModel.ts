import CompanyBasicInfoModel from "./CompanyBasicInfoModel";

export default interface CompaniesResponseModel{
    empresas: Array<CompanyBasicInfoModel>;
    cantidadPaginas: number;
}