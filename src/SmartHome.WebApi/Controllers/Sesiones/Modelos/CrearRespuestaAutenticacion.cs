namespace SmartHome.WebApi.Controllers.Autenticaciones.Modelos;

public readonly struct CrearRespuestaAutenticacion(string token, List<string> permisos)
{
    public string Token { get; init; } = token;
    public List<string> Permisos { get; init; } = permisos;
}
