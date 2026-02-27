using Microsoft.AspNetCore.Mvc;
using SmartHome.LogicaNegocio.Dispositivos;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.WebApi.Controllers.Importadores.Modelos;
using SmartHome.WebApi.Filtros;

namespace SmartHome.WebApi.Controllers.Importadores;

[ApiController]
[Route("importadores")]
[AutenticacionFiltro]
public sealed class ControladorImportador(IDispositivoLogica logicaDispositivo, ISesionLogica logicaSesion)
    : ControllerBase
{
    [AutorizacionFiltro("CrearDispositivos")]
    [HttpPost("dispositivos")]
    public void ImportarDispositivos(CrearSolicitudImportacion solicitud, [FromHeader] string authorization)
    {
        var usuario = logicaSesion.ObtenerUsuarioPorToken(authorization);

        logicaDispositivo.ImportarDispositivos(solicitud.Ruta, solicitud.IdentificadorImportador, usuario.Empresa!);
        logicaDispositivo.GuardarCambios();
    }

    [AutorizacionFiltro("CrearDispositivos")]
    [HttpGet]
    public List<InformacionRespuestaImportadores> ObtenerImportadores()
    {
        var implementaciones = logicaDispositivo.ObtenerIdentificadoresDeImportadores();
        return implementaciones.ConvertAll(v => new InformacionRespuestaImportadores(v));
    }
}
