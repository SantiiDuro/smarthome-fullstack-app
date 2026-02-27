import ImagesModel from "./ImagesModel";

export default interface SensorCreateModel {
    nombre: string;
    modelo: string;
    descripcion: string;
    fotografias: Array<ImagesModel>;
}