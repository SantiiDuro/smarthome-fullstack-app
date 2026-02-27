export default interface UserBasicInfoModel {
    nombre: string;
    apellido: string;
    nombreCompleto: string;
    tipoRol: string;
    fechaCreacion: Date | string | null;
    email: string;
}