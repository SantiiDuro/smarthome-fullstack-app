using Microsoft.AspNetCore.Mvc;
using SmartHome.LogicaNegocio;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.LogicaNegocio.Usuarios;
using SmartHome.LogicaNegocio.Usuarios.Entidades;
using SmartHome.WebApi.Controllers.Usuarios.Modelos;
using SmartHome.WebApi.Filtros;

namespace SmartHome.WebApi.Controllers.Usuarios;

[ApiController]
[Route("usuarios")]
[AutenticacionFiltro]
public sealed class ControladorUsuario(IUsuarioLogica logicaUsuario, ISesionLogica logicaSesion)
: ControllerBase
{
    [HttpGet]
    [AutorizacionFiltro("ListarUsuarios")]
    public RespuestaUsuarios ObtenerTodos([FromQuery] ParametroPaginacion paginacionParametros,
        [FromQuery] ParametroUsuarioFiltro filtroParametros)
    {
        var resultado = logicaUsuario.ObtenerTodos(paginacionParametros, filtroParametros);
        var usuarios = resultado.Usuarios.ConvertAll(u => new InformacionRespuestaUsuario(u));

        return new RespuestaUsuarios(usuarios, resultado.CantidadPaginas);
    }

    [HttpPatch("permisos")]
    [AutorizacionFiltro("ActualizarRolUsuario")]
    public InformacionRespuestaActualizarRol ActualizarRol([FromHeader] string authorization)
    {
        var usuario = logicaSesion.ObtenerUsuarioPorToken(authorization);
        logicaUsuario.ActualizarRol(usuario);

        var permisos = usuario.Rol.Permisos.Select(p => p.ToString()).ToList();

        return new InformacionRespuestaActualizarRol(permisos);
    }

    [HttpPatch("foto-perfil")]
    [AutorizacionFiltro("ActualizarRolUsuario")]
    public void ActualizarFotoPerfil(SolicitudActualizarFotoPerfil solicitud, [FromHeader] string authorization)
    {
        var usuario = logicaSesion.ObtenerUsuarioPorToken(authorization);
        logicaUsuario.ActualizarFotoPerfil(usuario, solicitud.FotoPerfil);
    }
}
