using Microsoft.AspNetCore.Mvc;
using SmartHome.LogicaNegocio.Dispositivos;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.WebApi.Controllers.Lamparas.Modelos;
using SmartHome.WebApi.Filtros;

namespace SmartHome.WebApi.Controllers.Lamparas;

[ApiController]
[AutenticacionFiltro]
[Route("lamparas")]
public sealed class ControladorLampara(IDispositivoLogica logicaDispositivo, ISesionLogica logicaSesion)
    : ControllerBase
{
    [HttpPost]
    [AutorizacionFiltro("CrearDispositivos")]
    public void AgregarLampara(CrearSolicitudLampara solicitud, [FromHeader] string authorization)
    {
        var usuario = logicaSesion.ObtenerUsuarioPorToken(authorization);
        var args = solicitud.Args(usuario);

        logicaDispositivo.AgregarLampara(args);

        logicaDispositivo.GuardarCambios();
    }
}
