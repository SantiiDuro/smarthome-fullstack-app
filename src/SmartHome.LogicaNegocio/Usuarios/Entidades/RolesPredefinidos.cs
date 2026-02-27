namespace SmartHome.LogicaNegocio.Usuarios.Entidades;
public static class RolesPredefinidos
{
    public static Guid ID_DUEÑO_HOGAR = Guid.Parse("030c21ec-8635-48e3-af7e-68fda450daca");
    public static Guid ID_ADMIN = Guid.Parse("030c21ec-8635-48e3-af7e-68fda450dacb");
    public static Guid ID_DUEÑO_EMPRESA = Guid.Parse("030c21ec-8635-48e3-af7e-68fda450dacc");
    public static Guid ID_ADMIN_DUEÑO_HOGAR = Guid.Parse("030c21ec-8635-48e3-af7e-68fda450dacd");
    public static Guid ID_DUEÑO_EMPRESA_Y_HOGAR = Guid.Parse("030c21ec-8635-48e3-af7e-68fda450dace");

    public static readonly Rol DueñoHogar = new Rol
    {
        Id = ID_DUEÑO_HOGAR,
        Tipo = "dueño hogar",
        Permisos = [PermisoUsuario.CrearHogar]
    };

    public static readonly Rol Admin = new Rol
    {
        Id = ID_ADMIN,
        Tipo = "administrador",
        Permisos = [PermisoUsuario.CrearAdmin, PermisoUsuario.EliminarAdmin, PermisoUsuario.ListarEmpresas, PermisoUsuario.ListarUsuarios, PermisoUsuario.CrearDueñoEmpresa, PermisoUsuario.ActualizarRolUsuario]
    };

    public static readonly Rol DueñoEmpresa = new Rol
    {
        Id = ID_DUEÑO_EMPRESA,
        Tipo = "dueño empresa",
        Permisos = [PermisoUsuario.CrearEmpresa, PermisoUsuario.CrearDispositivos, PermisoUsuario.ActualizarRolUsuario]
    };

    public static readonly Rol AdminDueñoHogar = new Rol
    {
        Id = ID_ADMIN_DUEÑO_HOGAR,
        Tipo = "administrador dueño hogar",
        Permisos = [PermisoUsuario.CrearAdmin, PermisoUsuario.EliminarAdmin, PermisoUsuario.ListarEmpresas, PermisoUsuario.ListarUsuarios, PermisoUsuario.CrearDueñoEmpresa, PermisoUsuario.CrearHogar]
    };

    public static readonly Rol DueñoEmpresaYHogar = new Rol
    {
        Id = ID_DUEÑO_EMPRESA_Y_HOGAR,
        Tipo = "dueño empresa y hogar",
        Permisos = [PermisoUsuario.CrearEmpresa, PermisoUsuario.CrearDispositivos, PermisoUsuario.CrearHogar]
    };
}
