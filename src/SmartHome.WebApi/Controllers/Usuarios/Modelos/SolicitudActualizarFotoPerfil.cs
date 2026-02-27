namespace SmartHome.WebApi.Controllers.Usuarios.Modelos;

public record class SolicitudActualizarFotoPerfil
{
    public string FotoPerfil { get; init; } = null!;
}
