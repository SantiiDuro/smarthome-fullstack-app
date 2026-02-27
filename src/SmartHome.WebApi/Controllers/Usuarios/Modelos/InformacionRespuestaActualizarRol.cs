namespace SmartHome.WebApi.Controllers.Usuarios.Modelos;

public readonly struct InformacionRespuestaActualizarRol(List<string> permisos)
{
    public List<string> Permisos { get; } = permisos;
}
