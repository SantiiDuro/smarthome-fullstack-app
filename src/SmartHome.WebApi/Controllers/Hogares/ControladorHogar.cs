using Microsoft.AspNetCore.Mvc;
using SmartHome.LogicaNegocio.Cuartos;
using SmartHome.LogicaNegocio.Dispositivos;
using SmartHome.LogicaNegocio.DispositivosHogar;
using SmartHome.LogicaNegocio.DispositivosHogar.Entidades;
using SmartHome.LogicaNegocio.Empresas;
using SmartHome.LogicaNegocio.Hogares;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.LogicaNegocio.Usuarios;
using SmartHome.WebApi.Controllers.Hogares.Modelos;
using SmartHome.WebApi.Filtros;

namespace SmartHome.WebApi.Controllers.Hogares;

[ApiController]
[Route("hogares")]
[AutenticacionFiltro]
public sealed class ControladorHogar
    (IHogarLogica logicaHogar, IUsuarioLogica logicaUsuario, ISesionLogica logicaSesion, IDispositivoHogarLogica logicaDispositivoHogar, IDispositivoLogica logicaDispositivo, IEmpresaLogica logicaEmpresa, ICuartoLogica logicaCuarto)
    : ControllerBase
{
    [HttpPost]
    [AutorizacionFiltro("CrearHogar")]
    public void Crear(CrearSolicitudHogar solicitud, [FromHeader] string authorization)
    {
        var usuario = logicaSesion.ObtenerUsuarioPorToken(authorization);

        var args = solicitud.Args(usuario);

        logicaHogar.Agregar(args);
        logicaHogar.GuardarCambios();
    }

    [HttpPost("{id}/miembros")]
    public void AgregarMiembro(CrearSolicitudAgregarMiembro solicitud, string id, [FromHeader] string authorization)
    {
        var usuario = logicaSesion.ObtenerUsuarioPorToken(authorization);
        var miembro = solicitud.ObtenerMiembro(logicaUsuario, logicaHogar, id);

        logicaHogar.AgregarMiembro(id, miembro, usuario);
    }

    [HttpPost("{id}/dispositivos")]
    public void AsociarDispositivo(CrearSolicitudAsociarDispositivo solicitud, string id, [FromHeader] string authorization)
    {
        var usuario = logicaSesion.ObtenerUsuarioPorToken(authorization);
        var args = solicitud.Args(logicaDispositivo, logicaHogar, id);

        logicaDispositivoHogar.Agregar(args, usuario);
    }

    [HttpGet("{id}/dispositivos")]
    public List<InformacionRespuestaListarDispositivos> ListarDispositivos(string id, [FromHeader] string authorization,
        [FromQuery] ParametroDispositivoHogarFiltro filtroParametros)
    {
        var usuario = logicaSesion.ObtenerUsuarioPorToken(authorization);

        var dispositivosHogar = logicaDispositivoHogar.ObtenerDispositivosDeHogar(id, usuario, filtroParametros);

        return dispositivosHogar.ConvertAll(dh => new InformacionRespuestaListarDispositivos(logicaEmpresa, dh));
    }

    [HttpGet("{id}/miembros")]
    public List<InformacionRespuestaListarMiembros> ListarMiembros(string id, [FromHeader] string authorization)
    {
        var usuario = logicaSesion.ObtenerUsuarioPorToken(authorization);

        var miembros = logicaHogar.ObtenerMiembrosDeHogar(id, usuario);

        return miembros.ConvertAll(m => new InformacionRespuestaListarMiembros(logicaUsuario, m));
    }

    [HttpPatch("{id}/alias")]
    public void Modificar(CrearSolicitudModificarHogar solicitud, string id, [FromHeader] string authorization)
    {
        var usuario = logicaSesion.ObtenerUsuarioPorToken(authorization);

        var alias = solicitud.ObtenerAlias();

        logicaHogar.ActualizarAlias(id, alias, usuario);
    }

    [HttpPost("{id}/cuartos")]
    public void AgregarCuarto(CrearSolicitudAgregarCuarto solicitud, string id, [FromHeader] string authorization)
    {
        var usuario = logicaSesion.ObtenerUsuarioPorToken(authorization);
        var hogar = logicaHogar.ObtenerPorId(id);
        var cuartoArgs = solicitud.Args(hogar);

        logicaCuarto.Agregar(cuartoArgs, usuario);
    }

    [HttpGet("usuario")]
    public List<InformacionRespuestaListarHogares> ListarHogaresDeUsuario([FromHeader] string authorization)
    {
        var usuario = logicaSesion.ObtenerUsuarioPorToken(authorization);

        var hogares = logicaHogar.ObtenerHogaresPorUsuario(usuario);

        return hogares.ConvertAll(h => new InformacionRespuestaListarHogares(h));
    }

    [HttpGet("{id}/cuartos")]
    public List<InformacionRespuestaListarCuartos> ListarCuartos(string id, [FromHeader] string authorization)
    {
        var usuario = logicaSesion.ObtenerUsuarioPorToken(authorization);

        var cuartos = logicaHogar.ObtenerCuartosDeHogar(id, usuario);
        return cuartos.ConvertAll(c => new InformacionRespuestaListarCuartos(c));
    }
}
