export default interface MemberBasicInfoModel {
    nombreCompleto: string;
    email: string;
    fotoPerfil: string;
    tienePermisoListarDispositivos: boolean;
    tienePermisoAsociarDispositivos: boolean;
    recibeNotificaciones: boolean;
    tienePermisoAdministrarCuartos: boolean;
    tienePermisoModificarNombreDispositivos: boolean;
}