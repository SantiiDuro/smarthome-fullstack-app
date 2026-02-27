using System.Linq.Expressions;
using SmartHome.LogicaNegocio.Cuartos.Entidades;
using SmartHome.LogicaNegocio.Hogares.Entidades;
using SmartHome.LogicaNegocio.Hogares.Fabrica;
using SmartHome.LogicaNegocio.Notificaciones.Entidades;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.LogicaNegocio.Hogares;
public sealed class HogarLogica(IHogarRepositorio repositorioHogar)
    : IHogarLogica
{
    private const string PermisoAgregarMiembro = "AgregarMiembro";
    private const string PermisoListarMiembros = "ListarMiembro";
    private const string PermisoModificarAlias = "ModificarAlias";
    private const string PermisoAdministrarCuartos = "AdministrarCuarto";

    public void GuardarCambios()
    {
        repositorioHogar.GuardarCambios();
    }

    public Hogar Agregar(CrearHogaresArgs args)
    {
        var hogar = new Hogar
        {
            Calle = args.Calle,
            NumPuerta = args.NumPuerta,
            Latitud = args.Latitud,
            Longitud = args.Longitud,
            CantMiembrosSoportados = args.CantMiembrosSoportados,
            Miembros = [],
            DueñoId = args.DueñoId,
            Alias = args.Alias
        };
        repositorioHogar.Agregar(hogar);

        return hogar;
    }

    public Hogar AgregarMiembro(string hogarId, MiembroHogar miembro, Usuario usuario)
    {
        var hogar = ObtenerPorId(hogarId);

        ValidacionesMiembro(hogar, miembro);

        if (VerificarPermiso(PermisoAgregarMiembro, usuario, hogarId))
        {
            repositorioHogar.AgregarMiembro(miembro);

            hogar.Miembros.Add(miembro);

            return hogar;
        }

        throw new InvalidOperationException("Solo el dueño del hogar puede agregar nuevos miembros");
    }

    private void ValidacionesMiembro(Hogar hogar, MiembroHogar miembro)
    {
        if (hogar.DueñoId == miembro.MiembroId)
        {
            throw new InvalidOperationException("El dueño del hogar no puede ser agregado como miembro.");
        }

        if (hogar.Miembros.Count + 1 >= hogar.CantMiembrosSoportados)
        {
            throw new InvalidOperationException("El hogar ya tiene el maximo de miembros.");
        }

        if (!miembro.Miembro.Rol.TienePermiso("CrearHogar"))
        {
            throw new InvalidOperationException("El usuario a agregar no tiene cuenta de dueño de hogar.");
        }

        if (hogar.Miembros.Any(m => m.MiembroId == miembro.MiembroId || m.MiembroId == hogar.DueñoId))
        {
            throw new InvalidOperationException("El miembro ya forma parte del hogar.");
        }
    }

    public List<Hogar> ObtenerTodos()
    {
        return repositorioHogar.ObtenerTodos();
    }

    public Hogar ObtenerPorId(string id)
    {
        if (Guid.TryParse(id, out var idGuid))
        {
            var hogar = ObtenerTodos().FirstOrDefault(h => h.Id == idGuid);
            if (hogar is null)
            {
                throw new KeyNotFoundException("El hogar no existe.");
            }

            return hogar;
        }

        throw new FormatException("El id no tiene el formato correto");
    }

    public bool EsDueñoHogar(Usuario usuario, string id)
    {
        if (Guid.TryParse(id, out var _))
        {
            if (ObtenerPorId(id).DueñoId == usuario.Id)
            {
                return true;
            }
        }

        return false;
    }

    public bool Existe(Expression<Func<Hogar, bool>> predicado)
    {
        return repositorioHogar.Existe(predicado);
    }

    public bool TienePermisoAsociarDispositivo(Usuario usuario, string id)
    {
        if (Guid.TryParse(id, out var _))
        {
            var miembros = ObtenerPorId(id).Miembros;
            if (miembros.Any(m => m.MiembroId == usuario.Id))
            {
                var miembro = ObtenerPorId(id).Miembros.Where(m => m.MiembroId == usuario.Id).FirstOrDefault();
                if (miembro is not null && miembro.PermisoAsociarDispositivos)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool TienePermisoListarDispositivos(Usuario usuario, string id)
    {
        if (Guid.TryParse(id, out var _))
        {
            var miembros = ObtenerPorId(id).Miembros;
            if (miembros.Any(m => m.MiembroId == usuario.Id))
            {
                var miembro = ObtenerPorId(id).Miembros.Where(m => m.MiembroId == usuario.Id).FirstOrDefault();
                if (miembro is not null && miembro.PermisoListarDispositivos)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool TienePermisoAdministrarCuartos(Usuario usuario, string id)
    {
        if (Guid.TryParse(id, out var _))
        {
            var miembros = ObtenerPorId(id).Miembros;
            if (miembros.Any(m => m.MiembroId == usuario.Id))
            {
                var miembro = ObtenerPorId(id).Miembros.Where(m => m.MiembroId == usuario.Id).FirstOrDefault();
                if (miembro is not null && miembro.PermisoAdministrarCuartos)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool TienePermisoModificarNombreDispositivos(Usuario usuario, string id)
    {
        if (Guid.TryParse(id, out var _))
        {
            var miembros = ObtenerPorId(id).Miembros;
            if (miembros.Any(m => m.MiembroId == usuario.Id))
            {
                var miembro = ObtenerPorId(id).Miembros.Where(m => m.MiembroId == usuario.Id).FirstOrDefault();
                if (miembro is not null && miembro.PermisoModificarNombreDispositivos)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public List<MiembroHogar> ObtenerMiembrosHogarConNotificaciones(Guid hogarId)
    {
        var hogar = ObtenerPorId(hogarId.ToString());
        var miembros = hogar.Miembros;
        return miembros.Where(m => m.PermisoNotificaciones).ToList();
    }

    public void ActualizarNotificacionesDeMiembros(Guid hogarId, List<Notificacion> notificaciones)
    {
        var hogar = ObtenerPorId(hogarId.ToString());

        foreach (var n in notificaciones)
        {
            var miembro = hogar.Miembros.Where(m => m.MiembroId == n.Miembro.MiembroId).FirstOrDefault();
            if (miembro == null)
            {
                throw new KeyNotFoundException($"No se encontró un miembro con el ID '{n.Miembro.MiembroId}' en el hogar.");
            }

            miembro.Notificaciones.Add(n);
            repositorioHogar.ActualizarMiembro(miembro);
        }
    }

    public void ActualizarAlias(string id, string alias, Usuario usuario)
    {
        if (VerificarPermiso(PermisoModificarAlias, usuario, id))
        {
            var hogar = ObtenerPorId(id);
            hogar.Alias = alias;
            repositorioHogar.Actualizar(hogar);
        }
        else
        {
            throw new InvalidOperationException("Solo el dueño del hogar puede modificar el alias");
        }
    }

    public List<Hogar> ObtenerHogaresPorUsuario(Usuario usuario)
    {
        var hogares = ObtenerTodos();

        return hogares.Where(h =>
            h.Miembros.Any(m => m.MiembroId == usuario.Id) ||
            h.DueñoId == usuario.Id)
            .ToList();
    }

    public List<MiembroHogar> ObtenerMiembrosDeHogar(string hogarId, Usuario usuario)
    {
        if (VerificarPermiso(PermisoListarMiembros, usuario, hogarId))
        {
            var hogar = ObtenerPorId(hogarId);

            return hogar.Miembros;
        }

        throw new InvalidOperationException("No tienes permiso para listar los miembros de este hogar");
    }

    public List<Cuarto> ObtenerCuartosDeHogar(string hogarId, Usuario usuario)
    {
        if (VerificarPermiso(PermisoAdministrarCuartos, usuario, hogarId))
        {
            var hogar = ObtenerPorId(hogarId);
            return hogar.Cuartos;
        }

        throw new InvalidOperationException("No tienes permiso para administrar los cuartos de este hogar");
    }

    public bool VerificarPermiso(string accion, Usuario usuario, string idHogar)
    {
        var estrategia = PermisoEstrategiaFactory.CrearEstrategia(accion);

        return estrategia.TienePermiso(usuario, idHogar, this);
    }
}
