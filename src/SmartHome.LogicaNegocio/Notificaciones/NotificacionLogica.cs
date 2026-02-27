using SmartHome.LogicaNegocio.DispositivosHogar;
using SmartHome.LogicaNegocio.Hogares;
using SmartHome.LogicaNegocio.Notificaciones.Entidades;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.LogicaNegocio.Notificaciones;
public class NotificacionLogica(INotificacionRepositorio repositorioNotificacion, IHogarLogica logicaHogar, IDispositivoHogarLogica logicaDispositivoHogar)
    : INotificacionLogica
{
    public Notificacion Agregar(CrearNotificacionesArgs args)
    {
        var notificacion = new Notificacion
        {
            DispositivoHogar = args.DispositivoHogar,
            Miembro = args.Miembro,
            MiembroId = args.Miembro.Id,
            Evento = args.Evento,
            FueLeida = args.FueLeida,
            FechaHora = args.FechaHora
        };

        repositorioNotificacion.Agregar(notificacion);
        return notificacion;
    }

    public void GuardarCambios()
    {
        repositorioNotificacion.GuardarCambios();
    }

    public List<Notificacion> GenerarNotificaciones(string evento, string dispositivoHogarId)
    {
        var dispositivoHogar = logicaDispositivoHogar.ObtenerDispositivoHogarPorId(dispositivoHogarId);
        var miembros = logicaHogar.ObtenerMiembrosHogarConNotificaciones(dispositivoHogar.HogarId);

        if (!dispositivoHogar.EstaConectado)
        {
            throw new InvalidOperationException("El dispositivo no está en linea");
        }

        var notificaciones = new List<Notificacion>();

        foreach (var miembro in miembros)
        {
            var notificacionArgs = new CrearNotificacionesArgs(
                dispositivoHogar,
                miembro,
                evento);

            notificaciones.Add(Agregar(notificacionArgs));
        }

        GuardarCambios();
        logicaHogar.ActualizarNotificacionesDeMiembros(dispositivoHogar.HogarId, notificaciones);

        return notificaciones;
    }

    public List<Notificacion> ObtenerNotificacionesPorUsuario(Usuario usuario, ParametroNotificacionFiltro? parametroFiltro)
    {
        return repositorioNotificacion.ObtenerTodos(parametroFiltro)
            .Where(n => n.Miembro.MiembroId == usuario.Id)
            .ToList();
    }

    public void MarcarComoLeidas(List<Notificacion> notificaciones)
    {
        foreach (var n in notificaciones)
        {
            n.FueLeida = true;
            repositorioNotificacion.Actualizar(n);
        }
    }
}
