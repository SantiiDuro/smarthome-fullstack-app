using SmartHome.LogicaNegocio.DispositivosHogar.Entidades;
using SmartHome.LogicaNegocio.Hogares.Entidades;

namespace SmartHome.LogicaNegocio.Notificaciones.Entidades;
public class Notificacion
{
    public Guid Id { get; init; }
    public DispositivoHogar DispositivoHogar { get; init; } = null!;
    public MiembroHogar Miembro { get; init; } = null!;
    public Guid MiembroId { get; init; }
    public string Evento { get; init; } = null!;
    public bool FueLeida { get; set; }
    public DateTime FechaHora { get; init; }

    public Notificacion()
    {
        Id = Guid.NewGuid();
    }
}
