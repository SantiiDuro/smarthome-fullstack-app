import DeviceBasicInfoModel from "./DeviceBasicInfoModel";

export default interface DevicesResponseModel {
    dispositivos: Array<DeviceBasicInfoModel>;
    cantidadPaginas: number;
}