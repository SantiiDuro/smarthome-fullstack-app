export default interface HomeDeviceBasicInfoModel {
    id: string;
    nombre: string;
    modelo: string;
    fotoPrincipal: string;
    nombreEmpresa: string;
    estaConectado: boolean;
    estaAbierto: boolean | null;
    estaEncendida: boolean | null;
}