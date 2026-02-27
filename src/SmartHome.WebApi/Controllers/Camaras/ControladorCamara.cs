using Microsoft.AspNetCore.Mvc;
using SmartHome.LogicaNegocio.Dispositivos;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.WebApi.Controllers.Camaras.Modelos;
using SmartHome.WebApi.Filtros;

namespace SmartHome.WebApi.Controllers.Camaras;

[ApiController]
[Route("camaras")]
[AutenticacionFiltro]

public class ControladorCamara(IDispositivoLogica logicaDispositivo, ISesionLogica logicaSesion)
    : ControllerBase
{
    [HttpPost]
    [AutorizacionFiltro("CrearDispositivos")]
    public void AgregarCamara(CrearSolicitudCamara solicitud, [FromHeader] string authorization)
    {
        var usuario = logicaSesion.ObtenerUsuarioPorToken(authorization);
        var args = solicitud.Args(usuario);

        logicaDispositivo.AgregarCamara(args);

        logicaDispositivo.GuardarCambios();
    }
}
