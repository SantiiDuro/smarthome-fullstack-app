using SmartHome.LogicaNegocio.Notificaciones.Entidades;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.LogicaNegocio.Notificaciones;
public interface INotificacionLogica
{
    Notificacion Agregar(CrearNotificacionesArgs args);
    List<Notificacion> GenerarNotificaciones(string evento, string dispositivoHogarId);
    List<Notificacion> ObtenerNotificacionesPorUsuario(Usuario usuario, ParametroNotificacionFiltro? parametroFiltro);
    void MarcarComoLeidas(List<Notificacion> notificaciones);
    void GuardarCambios();
}
