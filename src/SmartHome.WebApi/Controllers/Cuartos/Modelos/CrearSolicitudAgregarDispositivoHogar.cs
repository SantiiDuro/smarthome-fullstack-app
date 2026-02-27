namespace SmartHome.WebApi.Controllers.Cuartos.Modelos;

public sealed record CrearSolicitudAgregarDispositivoHogar
{
    public string DispositivoHogarId { get; init; } = null!;

    public string ObtenerDispositivoHogarId()
    {
        return DispositivoHogarId;
    }
}
