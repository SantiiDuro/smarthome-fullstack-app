using Microsoft.AspNetCore.Mvc;
using SmartHome.LogicaNegocio.Dispositivos;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.WebApi.Controllers.Sensores.Modelos;
using SmartHome.WebApi.Filtros;

namespace SmartHome.WebApi.Controllers.Sensores;

[ApiController]
[AutenticacionFiltro]
[Route("sensores-movimiento")]
public sealed class ControladorSensorMovimiento(IDispositivoLogica logicaDispositivo, ISesionLogica logicaSesion)
    : ControllerBase
{
    [HttpPost]
    [AutorizacionFiltro("CrearDispositivos")]
    public void AgregarSensorMovimiento(CrearSolicitudSensor solicitud, [FromHeader] string authorization)
    {
        var usuario = logicaSesion.ObtenerUsuarioPorToken(authorization);
        var args = solicitud.Args(usuario);

        logicaDispositivo.AgregarSensorMovimiento(args);

        logicaDispositivo.GuardarCambios();
    }
}
