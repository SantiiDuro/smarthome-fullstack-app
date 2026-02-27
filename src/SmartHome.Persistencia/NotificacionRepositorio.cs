using Microsoft.EntityFrameworkCore;
using SmartHome.LogicaNegocio.Dispositivos.Entidades;
using SmartHome.LogicaNegocio.Notificaciones;
using SmartHome.LogicaNegocio.Notificaciones.Entidades;

namespace SmartHome.Persistencia;
public class NotificacionRepositorio(ContextoSql contexto)
    : INotificacionRepositorio
{
    private readonly ContextoSql _contexto = contexto;

    public void GuardarCambios()
    {
        _contexto.SaveChanges();
    }

    public void Agregar(Notificacion notificacion)
    {
        _contexto.Notificaciones.Add(notificacion);
    }

    public List<Notificacion> ObtenerTodos(ParametroNotificacionFiltro? parametroFiltro)
    {
        var query = _contexto.Notificaciones.Include(n => n.Miembro)
            .Include(n => n.DispositivoHogar).AsQueryable();

        if (parametroFiltro != null)
        {
            if (!string.IsNullOrEmpty(parametroFiltro.TipoDispositivo))
            {
                query = Enum.TryParse<TipoDispositivo>(parametroFiltro.TipoDispositivo, true, out var tipoDispositivoEnum)
                    ? query.Where(x => x.DispositivoHogar.Dispositivo.Tipo == tipoDispositivoEnum) : query.Where(x => false);
            }

            if (parametroFiltro.FechaDeCreacion != default)
            {
                query = query.Where(x => x.FechaHora.Date == parametroFiltro.FechaDeCreacion.Date);
            }

            if (!string.IsNullOrEmpty(parametroFiltro.Leida))
            {
                query = bool.TryParse(parametroFiltro.Leida, out var leida)
                    ? query.Where(x => x.FueLeida == leida) : query.Where(x => false);
            }
        }

        return query.ToList();
    }

    public void Actualizar(Notificacion notificacion)
    {
        _contexto.Notificaciones.Update(notificacion);
        _contexto.SaveChanges();
    }
}
