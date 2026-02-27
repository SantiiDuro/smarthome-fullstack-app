using Microsoft.AspNetCore.Mvc;
using SmartHome.LogicaNegocio.Cuartos;
using SmartHome.LogicaNegocio.DispositivosHogar;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.WebApi.Controllers.Cuartos.Modelos;
using SmartHome.WebApi.Filtros;

namespace SmartHome.WebApi.Controllers.Cuartos;

[ApiController]
[Route("cuartos")]
[AutenticacionFiltro]
public sealed class ControladorCuarto(ISesionLogica logicaSesion, IDispositivoHogarLogica logicaDispositivoHogar, ICuartoLogica logicaCuarto)
    : ControllerBase
{
    [HttpPost("{id}/dispositivos-hogar")]
    public void AgregarDispositivoHogar(CrearSolicitudAgregarDispositivoHogar solicitud, string id, [FromHeader] string authorization)
    {
        var usuario = logicaSesion.ObtenerUsuarioPorToken(authorization);

        var cuarto = logicaCuarto.ObtenerPorId(id);

        logicaDispositivoHogar.AgregarACuarto(solicitud.ObtenerDispositivoHogarId(), cuarto, usuario);
    }
}
