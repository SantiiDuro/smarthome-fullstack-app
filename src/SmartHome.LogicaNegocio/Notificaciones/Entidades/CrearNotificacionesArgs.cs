using SmartHome.LogicaNegocio.DispositivosHogar.Entidades;
using SmartHome.LogicaNegocio.Hogares.Entidades;

namespace SmartHome.LogicaNegocio.Notificaciones.Entidades;
public sealed class CrearNotificacionesArgs
{
    public readonly DispositivoHogar DispositivoHogar;
    public readonly MiembroHogar Miembro;
    public readonly string Evento;
    public readonly bool FueLeida;
    public readonly DateTime FechaHora;

    public CrearNotificacionesArgs(
        DispositivoHogar dispositivoHogar,
        MiembroHogar miembro,
        string evento)
    {
        ArgumentNullException.ThrowIfNull(dispositivoHogar);

        ArgumentNullException.ThrowIfNull(miembro);

        if (string.IsNullOrEmpty(evento))
        {
            throw new ArgumentNullException(nameof(evento));
        }

        DispositivoHogar = dispositivoHogar;
        Miembro = miembro;
        Evento = evento;
        FueLeida = false;
        FechaHora = DateTime.Now;
    }
}
