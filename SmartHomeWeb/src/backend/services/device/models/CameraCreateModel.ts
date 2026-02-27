import ImagesModel from "./ImagesModel";

export default interface CameraCreateModel {
    nombre: string;
    modelo: string;
    descripcion: string;
    fotografias: Array<ImagesModel>;
    detectaMovimiento: boolean;
    detectaPersona: boolean;
    usoExterior: boolean;
    usoInterior: boolean;
}