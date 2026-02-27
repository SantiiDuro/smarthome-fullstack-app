export default interface HomeCreateModel {
    calle: string;
    numPuerta: number;
    latitud: number;
    longitud: number;
    cantMiembrosSoportados: number;
    alias: string | null;
}