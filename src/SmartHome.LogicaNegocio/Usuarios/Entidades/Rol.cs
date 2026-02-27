namespace SmartHome.LogicaNegocio.Usuarios.Entidades;

public sealed class Rol
{
    public Guid Id { get; init; }
    public string Tipo { get; init; } = null!;
    public List<PermisoUsuario> Permisos { get; init; } = null!;
    public Rol()
    {
        Id = Guid.NewGuid();
    }

    public bool TienePermiso(string permiso)
    {
        if (Enum.TryParse<PermisoUsuario>(permiso, out var permisoUsuario))
        {
            return Permisos.Contains(permisoUsuario);
        }

        return false;
    }
}
