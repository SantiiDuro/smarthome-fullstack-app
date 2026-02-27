using SmartHome.LogicaNegocio.Notificaciones.Entidades;

namespace SmartHome.LogicaNegocio.Notificaciones;
public interface INotificacionRepositorio
{
    void Agregar(Notificacion notificacion);
    void GuardarCambios();
    List<Notificacion> ObtenerTodos(ParametroNotificacionFiltro? parametroFiltro);
    void Actualizar(Notificacion notificacion);
}
