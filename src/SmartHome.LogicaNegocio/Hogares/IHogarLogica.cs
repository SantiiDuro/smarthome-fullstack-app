using System.Linq.Expressions;
using SmartHome.LogicaNegocio.Cuartos.Entidades;
using SmartHome.LogicaNegocio.Hogares.Entidades;
using SmartHome.LogicaNegocio.Notificaciones.Entidades;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.LogicaNegocio.Hogares;
public interface IHogarLogica
{
    void GuardarCambios();
    Hogar Agregar(CrearHogaresArgs hogar);
    Hogar AgregarMiembro(string hogarId, MiembroHogar miembro, Usuario usuario);
    bool EsDueñoHogar(Usuario usuario, string id);
    bool Existe(Expression<Func<Hogar, bool>> predicado);
    Hogar ObtenerPorId(string id);
    bool TienePermisoAsociarDispositivo(Usuario usuario, string id);
    bool TienePermisoListarDispositivos(Usuario usuario, string id);
    bool TienePermisoAdministrarCuartos(Usuario usuario, string id);
    bool TienePermisoModificarNombreDispositivos(Usuario usuario, string id);
    List<MiembroHogar> ObtenerMiembrosHogarConNotificaciones(Guid hogarId);
    void ActualizarNotificacionesDeMiembros(Guid hogarId, List<Notificacion> notificaciones);
    void ActualizarAlias(string id, string alias, Usuario usuario);
    List<Hogar> ObtenerHogaresPorUsuario(Usuario usuario);
    List<MiembroHogar> ObtenerMiembrosDeHogar(string hogarId, Usuario usuario);
    List<Cuarto> ObtenerCuartosDeHogar(string hogarId, Usuario usuario);
    bool VerificarPermiso(string accion, Usuario usuario, string idHogar);
}
