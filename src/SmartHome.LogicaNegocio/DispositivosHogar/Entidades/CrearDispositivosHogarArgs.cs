using SmartHome.LogicaNegocio.Dispositivos.Entidades;
using SmartHome.LogicaNegocio.Hogares.Entidades;

namespace SmartHome.LogicaNegocio.DispositivosHogar.Entidades;
public sealed record class CrearDispositivosHogarArgs
{
    public Dispositivo Dispositivo { get; init; } = null!;
    public Hogar Hogar { get; init; } = null!;
    public bool EstaConectado { get; init; }

    public CrearDispositivosHogarArgs(
        Dispositivo dispositivo,
        Hogar hogar)
    {
        ArgumentNullException.ThrowIfNull(dispositivo);

        ArgumentNullException.ThrowIfNull(hogar);

        Dispositivo = dispositivo;
        Hogar = hogar;
        EstaConectado = true;
    }
}
