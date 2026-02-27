using Microsoft.AspNetCore.Mvc;
using SmartHome.LogicaNegocio.DispositivosHogar;
using SmartHome.LogicaNegocio.Notificaciones;
using SmartHome.LogicaNegocio.Sesiones;
using SmartHome.WebApi.Controllers.DispositivosHogar.Modelos;
using SmartHome.WebApi.Filtros;

namespace SmartHome.WebApi.Controllers.DispositivosHogar;

[ApiController]
[Route("dispositivos-hogar")]
public sealed class ControladorDispositivosHogar(IDispositivoHogarLogica logicaDispositivoHogar, INotificacionLogica logicaNotificacion, ISesionLogica logicaSesion)
{
    private const string OperacionAbre = "Abre";
    private const string OperacionCierra = "Cierra";
    private const string OperacionEncender = "Encender";
    private const string OperacionApagar = "Apagar";

    private const string EventoAbierto = "Abierto";
    private const string EventoCerrado = "Cerrado";
    private const string EventoDeteccionMovimiento = "Detección movimiento";
    private const string EventoDeteccionPersona = "Detección persona";
    private const string EventoEncendida = "Encendida";
    private const string EventoApagada = "Apagada";

    [HttpPost("{id}/abrir")]
    public void SensorSeAbre(string id)
    {
        if (logicaDispositivoHogar.EjecutarOperacionDispositivo(id, OperacionAbre))
        {
            logicaNotificacion.GenerarNotificaciones(EventoAbierto, id);
        }
    }

    [HttpPost("{id}/cerrar")]
    public void SensorSeCierra(string id)
    {
        if (logicaDispositivoHogar.EjecutarOperacionDispositivo(id, OperacionCierra))
        {
            logicaNotificacion.GenerarNotificaciones(EventoCerrado, id);
        }
    }

    [HttpPost("{id}/movimiento-sensor")]
    public void SensorDetectaMovimiento(string id)
    {
        logicaDispositivoHogar.SensorDetectaMovimiento(id);

        logicaNotificacion.GenerarNotificaciones(EventoDeteccionMovimiento, id);
    }

    [AutenticacionFiltro]
    [HttpPost("{id}/conectar")]
    public void DispositivoHogarConectado(string id, [FromHeader] string authorization)
    {
        var usuario = logicaSesion.ObtenerUsuarioPorToken(authorization);

        logicaDispositivoHogar.Conectar(id, usuario);
    }

    [AutenticacionFiltro]
    [HttpPost("{id}/desconectar")]
    public void DispositivoHogarDesconectado(string id, [FromHeader] string authorization)
    {
        var usuario = logicaSesion.ObtenerUsuarioPorToken(authorization);

        logicaDispositivoHogar.Desconectar(id, usuario);
    }

    [HttpPost("{id}/movimiento-camara")]
    public void CamaraDetectaMovimiento(string id)
    {
        logicaDispositivoHogar.CamaraDetectaMovimiento(id);

        logicaNotificacion.GenerarNotificaciones(EventoDeteccionMovimiento, id);
    }

    [HttpPost("{id}/persona")]
    public void CamaraDetectaPersona(string id)
    {
        logicaDispositivoHogar.CamaraDetectaPersona(id);

        logicaNotificacion.GenerarNotificaciones(EventoDeteccionPersona, id);
    }

    [HttpPost("{id}/encender")]
    public void EncenderLampara(string id)
    {
        if (logicaDispositivoHogar.EjecutarOperacionDispositivo(id, OperacionEncender))
        {
            logicaNotificacion.GenerarNotificaciones(EventoEncendida, id);
        }
    }

    [HttpPost("{id}/apagar")]
    public void ApagarLampara(string id)
    {
        if (logicaDispositivoHogar.EjecutarOperacionDispositivo(id, OperacionApagar))
        {
            logicaNotificacion.GenerarNotificaciones(EventoApagada, id);
        }
    }

    [AutenticacionFiltro]
    [HttpPatch("{id}/nombre")]
    public void ActualizarNombreDispositivoHogar(string id, CrearSolicitudModificarNombreDh solicitud, [FromHeader] string authorization)
    {
        var usuario = logicaSesion.ObtenerUsuarioPorToken(authorization);

        var nombre = solicitud.ObtenerNombre();
        logicaDispositivoHogar.ActualizarNombreDispositivoHogar(id, nombre, usuario);
    }
}
