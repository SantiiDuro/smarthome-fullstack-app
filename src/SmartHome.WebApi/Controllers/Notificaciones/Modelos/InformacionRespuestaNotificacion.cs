using SmartHome.LogicaNegocio.Notificaciones.Entidades;

namespace SmartHome.WebApi.Controllers.Notificaciones.Modelos;

public record InformacionRespuestaNotificacion
{
    public string Evento { get; init; } = null!;
    public bool FueLeida { get; init; }
    public string FechaHora { get; init; }
    public string NombreDispositivoHogar { get; init; } = null!;

    public InformacionRespuestaNotificacion(Notificacion notificacion)
    {
        Evento = notificacion.Evento;
        FueLeida = notificacion.FueLeida;
        FechaHora = notificacion.FechaHora.ToString("G");
        NombreDispositivoHogar = notificacion.DispositivoHogar.Nombre;
    }
}
