using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.WebApi.Controllers.Usuarios.Modelos;

public record InformacionRespuestaUsuario
{
    public string Nombre { get; init; }
    public string Apellido { get; init; }
    public string NombreCompleto { get; init; }
    public string TipoRol { get; init; }
    public DateTime FechaCreacion { get; init; }
    public string Email { get; init; }

    public InformacionRespuestaUsuario(Usuario usuario)
    {
        Nombre = usuario.Nombre;
        Apellido = usuario.Apellido;
        NombreCompleto = usuario.Nombre + " " + usuario.Apellido;
        TipoRol = usuario.Rol.Tipo;
        FechaCreacion = usuario.FechaCreacion;
        Email = usuario.Email;
    }
}
