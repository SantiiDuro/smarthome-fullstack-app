using Microsoft.AspNetCore.Mvc;
using SmartHome.LogicaNegocio.Usuarios;
using SmartHome.WebApi.Controllers.DueñosEmpresa.Modelos;
using SmartHome.WebApi.Filtros;

namespace SmartHome.WebApi.Controllers.DueñosEmpresa;

[ApiController]
[Route("dueños-empresa")]
[AutenticacionFiltro]
public sealed class ControladorDueñoEmpresa(IUsuarioLogica logicaUsuario)
    : ControllerBase
{
    [AutorizacionFiltro("CrearDueñoEmpresa")]
    [HttpPost]
    public void Crear(CrearSolicitudDueñoEmpresa solicitud)
    {
        var args = solicitud.Args();

        logicaUsuario.AgregarDueñoEmpresa(args);
        logicaUsuario.GuardarCambios();
    }
}
