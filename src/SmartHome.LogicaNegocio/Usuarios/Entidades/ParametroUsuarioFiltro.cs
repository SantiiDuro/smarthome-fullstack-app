namespace SmartHome.LogicaNegocio.Usuarios.Entidades;

public class ParametroUsuarioFiltro
{
    public string Rol { get; set; } = null!;
    public string NombreCompleto { get; set; } = null!;

    public ParametroUsuarioFiltro(string rol, string nombreCompleto)
    {
        Rol = rol;
        NombreCompleto = nombreCompleto;
    }

    public ParametroUsuarioFiltro()
    {
    }
}
