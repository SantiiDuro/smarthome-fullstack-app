import UserBasicInfoModel from './UserBasicInfoModel';

export default interface UsersResposeModel {
    usuarios: Array<UserBasicInfoModel>;
    cantidadPaginas: number;
}