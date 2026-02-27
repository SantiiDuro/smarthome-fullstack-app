using Microsoft.AspNetCore.Mvc;
using SmartHome.LogicaNegocio.Hogares;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.LogicaNegocio.Usuarios;
using SmartHome.WebApi.Controllers.DueñosHogar.Modelos;
using SmartHome.WebApi.Filtros;

namespace SmartHome.WebApi.Controllers.DueñosHogar;

[ApiController]
[Route("dueños-hogar")]
public sealed class ControladorDueñoHogar(IUsuarioLogica logicaUsuario, IHogarLogica logicaHogar, ISesionLogica logicaSesion)
    : ControllerBase
{
    [HttpPost]
    public void Crear(CrearSolicitudDueñoHogar solicitud)
    {
        var args = solicitud.Args();

        logicaUsuario.AgregarDueñoHogar(args);
        logicaUsuario.GuardarCambios();
    }

    [AutenticacionFiltro]
    [HttpGet("{id}/permisos")]
    public InformacionRespuestaPermisosHogar ObtenerPermisosSobreHogar(string id, [FromHeader] string authorization)
    {
        var usuario = logicaSesion.ObtenerUsuarioPorToken(authorization);

        return new InformacionRespuestaPermisosHogar(usuario, id, logicaHogar);
    }
}
