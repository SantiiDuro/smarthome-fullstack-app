using SmartHome.LogicaNegocio.Dispositivos;
using SmartHome.LogicaNegocio.DispositivosHogar.Entidades;
using SmartHome.LogicaNegocio.Hogares;

namespace SmartHome.WebApi.Controllers.Hogares.Modelos;

public class CrearSolicitudAsociarDispositivo
{
    public string DispositivoId { get; init; } = null!;

    public CrearDispositivosHogarArgs Args(IDispositivoLogica logicaDispositivo, IHogarLogica logicaHogar, string idHogar)
    {
        var dispositivo = logicaDispositivo.ObtenerPorId(DispositivoId);
        var hogar = logicaHogar.ObtenerPorId(idHogar);

        return new CrearDispositivosHogarArgs(
            dispositivo,
            hogar);
    }
}
