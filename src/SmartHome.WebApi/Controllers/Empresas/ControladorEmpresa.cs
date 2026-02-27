using Microsoft.AspNetCore.Mvc;
using SmartHome.LogicaNegocio;
using SmartHome.LogicaNegocio.Empresas;
using SmartHome.LogicaNegocio.Empresas.Entidades;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.WebApi.Controllers.Empresas.Modelos;
using SmartHome.WebApi.Filtros;

namespace SmartHome.WebApi.Controllers.Empresas;

[ApiController]
[Route("empresas")]
[AutenticacionFiltro]
public sealed class ControladorEmpresa
    : ControllerBase
{
    private readonly IEmpresaLogica logicaEmpresa;
    private readonly ISesionLogica logicaSesion;

    public ControladorEmpresa(IEmpresaLogica logicaEmpresa, ISesionLogica logicaSesion)
    {
        this.logicaEmpresa = logicaEmpresa;
        this.logicaSesion = logicaSesion;
    }

    [AutorizacionFiltro("CrearEmpresa")]
    [HttpPost]
    public void Crear(CrearSolicitudEmpresa solicitud, [FromHeader] string authorization)
    {
        var args = solicitud.Args();

        var usuario = logicaSesion.ObtenerUsuarioPorToken(authorization);

        logicaEmpresa.Agregar(args, usuario);
        logicaEmpresa.GuardarCambios();
    }

    [AutorizacionFiltro("ListarEmpresas")]
    [HttpGet]
    public RespuestaEmpresas ObtenerTodos([FromQuery] ParametroPaginacion paginacionParametros, [FromQuery] ParametroEmpresaFiltro filtroParametros)
    {
        var resultado = logicaEmpresa.ObtenerTodos(paginacionParametros, filtroParametros);

        var empresas = resultado.Empresas.ConvertAll(e => new InformacionRespuestaEmpresa(e));

        return new RespuestaEmpresas(empresas, resultado.CantidadPaginas);
    }

    [AutorizacionFiltro("CrearEmpresa")]
    [HttpGet("validadores")]
    public List<InformacionRespuestaValidadores> ObtenerValidadores()
    {
        var implementaciones = logicaEmpresa.ObtenerIdentificadoresDeImplementaciones();
        return implementaciones.ConvertAll(v => new InformacionRespuestaValidadores(v));
    }
}
