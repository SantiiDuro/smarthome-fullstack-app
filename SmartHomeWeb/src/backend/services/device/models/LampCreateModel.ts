import ImagesModel from "./ImagesModel";

export default interface LampCreateModel {
    nombre: string;
    modelo: string;
    descripcion: string;
    fotografias: Array<ImagesModel>;
}