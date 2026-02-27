namespace SmartHome.LogicaNegocio.Usuarios.Entidades;

public sealed record class ObtenerUsuariosArgs
{
    public readonly List<Usuario> Usuarios = null!;
    public readonly int CantidadPaginas;

    public ObtenerUsuariosArgs(List<Usuario> usuarios, int cantidadPaginas)
    {
        Usuarios = usuarios;
        CantidadPaginas = cantidadPaginas;
    }
}
