using SmartHome.LogicaNegocio.Cuartos.Entidades;
using SmartHome.LogicaNegocio.Dispositivos.Entidades;
using SmartHome.LogicaNegocio.Hogares.Entidades;

namespace SmartHome.LogicaNegocio.DispositivosHogar.Entidades;
public sealed record class DispositivoHogar
{
    public Guid Id { get; init; }
    public string Nombre { get; set; } = null!;
    public Dispositivo Dispositivo { get; init; } = null!;
    public Guid DispositivoId { get; init; }
    public Hogar Hogar { get; init; } = null!;
    public Guid HogarId { get; init; }
    public Cuarto? Cuarto { get; set; }
    public Guid? CuartoId { get; set; }
    public bool EstaConectado { get; set; }
    public bool? EstaAbierto { get; set; }
    public bool? EstaEncendida { get; set; }

    public DispositivoHogar()
    {
        Id = Guid.NewGuid();
    }
}
