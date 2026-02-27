namespace SmartHome.LogicaNegocio.Usuarios.Entidades;

/// <summary>
/// Enumera los permisos que un usuario puede tener en el sistema.
/// </summary>
public enum PermisoUsuario
{
    /// <summary>
    /// Permite al usuario crear un hogar.
    /// </summary>
    CrearHogar,

    /// <summary>
    /// Permite al usuario crear un admin.
    /// </summary>
    CrearAdmin,

    /// <summary>
    /// Permite al usuario eliminar un admin.
    /// </summary>
    EliminarAdmin,

    /// <summary>
    /// Permite al usuario listar todas las empresas.
    /// </summary>
    ListarEmpresas,

    /// <summary>
    /// Permite al usuario listar todas las cuentas.
    /// </summary>
    ListarUsuarios,

    /// <summary>
    /// Permite al usuario crear una empresa.
    /// </summary>
    CrearEmpresa,

    /// <summary>
    /// Permite al usuario crear un dueño de empresa.
    /// </summary>
    CrearDueñoEmpresa,

    /// <summary>
    /// Permite al usuario crear dispositivos.
    /// </summary>
    CrearDispositivos,

    /// <summary>
    /// Permite al usuario obtener los permisos de dueño hogar.
    /// </summary>
    ActualizarRolUsuario,
}
