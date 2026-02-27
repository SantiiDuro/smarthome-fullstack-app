namespace SmartHome.WebApi.Controllers.Usuarios.Modelos;

public record class RespuestaUsuarios
{
    public List<InformacionRespuestaUsuario> Usuarios { get; init; }
    public int CantidadPaginas { get; init; }

    public RespuestaUsuarios(List<InformacionRespuestaUsuario> usuarios, int cantidadPaginas)
    {
        Usuarios = usuarios;
        CantidadPaginas = cantidadPaginas;
    }
}
