using Microsoft.AspNetCore.Mvc;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.LogicaNegocio.Usuarios;
using SmartHome.WebApi.Controllers.Administradores.Modelos;
using SmartHome.WebApi.Filtros;

namespace SmartHome.WebApi.Controllers.Administradores;

[ApiController]
[Route("administradores")]
[AutenticacionFiltro]
public sealed class ControladorAdministrador(IUsuarioLogica logicaUsuario, ISesionLogica logicaSesion)
    : ControllerBase
{
    [AutorizacionFiltro("CrearAdmin")]
    [HttpPost]
    public void Crear(CrearSolicitudAdministrador solicitud)
    {
        var args = solicitud.Args();

        logicaUsuario.AgregarAdmin(args);
        logicaUsuario.GuardarCambios();
    }

    [AutorizacionFiltro("EliminarAdmin")]
    [HttpDelete("{email}")]
    public void Eliminar(string email, [FromHeader] string authorization)
    {
        var usuario = logicaSesion.ObtenerUsuarioPorToken(authorization);

        logicaUsuario.EliminarAdmin(usuario, email);
    }
}
