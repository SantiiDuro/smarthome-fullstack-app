using Microsoft.AspNetCore.Mvc;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.LogicaNegocio.Usuarios;
using SmartHome.WebApi.Controllers.Autenticaciones.Modelos;

namespace SmartHome.WebApi.Controllers.Sesiones;

[ApiController]
[Route("sesiones")]
public sealed class ControladorSesion(IUsuarioLogica logicaUsuario, ISesionLogica logicaSesion)
    : ControllerBase
{
    [HttpPost]
    public CrearRespuestaAutenticacion Autenticar(CrearSolicitudAutenticacion solicitud)
    {
        var token = solicitud.ValidarSolicitudAutenticacion(logicaUsuario, logicaSesion);

        var usuario = logicaSesion.ObtenerUsuarioPorToken(token);

        var permisos = usuario.Rol.Permisos.Select(p => p.ToString()).ToList();

        return new CrearRespuestaAutenticacion(token, permisos);
    }

    [HttpDelete]
    public void Desautenticar([FromHeader] string authorization)
    {
        var usuario = logicaSesion.ObtenerUsuarioPorToken(authorization);
        logicaSesion.CerrarSesion(usuario);
    }
}
