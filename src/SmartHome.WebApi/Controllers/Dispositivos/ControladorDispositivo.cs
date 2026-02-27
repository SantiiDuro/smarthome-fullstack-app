using Microsoft.AspNetCore.Mvc;
using SmartHome.LogicaNegocio;
using SmartHome.LogicaNegocio.Dispositivos;
using SmartHome.LogicaNegocio.Dispositivos.Entidades;
using SmartHome.LogicaNegocio.Empresas;
using SmartHome.WebApi.Controllers.Dispositivos.Modelos;
using SmartHome.WebApi.Filtros;

namespace SmartHome.WebApi.Controllers.Dispositivos;

[ApiController]
[Route("dispositivos")]
[AutenticacionFiltro]
public sealed class ControladorDispositivo(IDispositivoLogica logicaDispositivo, IEmpresaLogica logicaEmpresa)
    : ControllerBase
{
    [HttpGet]
    public RespuestaDispositivos ObtenerTodos([FromQuery] ParametroPaginacion paginacionParametros,
        [FromQuery] ParametroDispositivoFiltro filtroParametros)
    {
        var resultado = logicaDispositivo.ObtenerTodos(paginacionParametros, filtroParametros);

        var dispositivos = resultado.Dispositivos.ConvertAll(d => new InformacionRespuestaDispositivo(logicaEmpresa, d));

        return new RespuestaDispositivos(dispositivos, resultado.CantidadPaginas);
    }

    [HttpGet("tipos")]
    public List<InformacionRespuestaTipoDispositivo> ObtenerTiposDeDispositivos()
    {
        var tiposDeDispositivos = logicaDispositivo.ObtenerTiposDeDispositivos();

        return tiposDeDispositivos.ConvertAll(t => new InformacionRespuestaTipoDispositivo(t));
    }
}
