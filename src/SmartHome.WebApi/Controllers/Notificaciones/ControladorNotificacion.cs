using Microsoft.AspNetCore.Mvc;
using SmartHome.LogicaNegocio.Notificaciones;
using SmartHome.LogicaNegocio.Notificaciones.Entidades;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.WebApi.Controllers.Notificaciones.Modelos;
using SmartHome.WebApi.Filtros;

namespace SmartHome.WebApi.Controllers.Notificaciones;

[ApiController]
[Route("notificaciones")]
[AutenticacionFiltro]
public sealed class ControladorNotificacion(INotificacionLogica logicaNotificacion, ISesionLogica logicaSesion)
    : ControllerBase
{
    [HttpGet]
    public List<InformacionRespuestaNotificacion> ObtenerNotificaciones([FromHeader] string authorization, [FromQuery] ParametroNotificacionFiltro filtroParametros)
    {
        var usuario = logicaSesion.ObtenerUsuarioPorToken(authorization);

        var notificaciones = logicaNotificacion.ObtenerNotificacionesPorUsuario(usuario, filtroParametros);

        return notificaciones.ConvertAll(n => new InformacionRespuestaNotificacion(n));
    }

    [HttpPatch("leidas")]
    public void MarcarNotificacionesComoLeidas([FromHeader] string authorization, [FromQuery] ParametroNotificacionFiltro filtroParametros)
    {
        var usuario = logicaSesion.ObtenerUsuarioPorToken(authorization);

        var notificaciones = logicaNotificacion.ObtenerNotificacionesPorUsuario(usuario, filtroParametros);

        logicaNotificacion.MarcarComoLeidas(notificaciones);
    }
}
