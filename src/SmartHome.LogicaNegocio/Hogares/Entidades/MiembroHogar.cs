using SmartHome.LogicaNegocio.Notificaciones.Entidades;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.LogicaNegocio.Hogares.Entidades;
public sealed record class MiembroHogar
{
    public Guid Id { get; init; }
    public Hogar Hogar { get; init; } = null!;
    public Guid HogarId { get; init; }
    public Usuario Miembro { get; init; } = null!;
    public Guid MiembroId { get; init; }
    public bool PermisoAsociarDispositivos { get; init; }
    public bool PermisoListarDispositivos { get; init; }
    public bool PermisoNotificaciones { get; init; }
    public bool PermisoAdministrarCuartos { get; init; }
    public bool PermisoModificarNombreDispositivos { get; init; }
    public List<Notificacion> Notificaciones { get; set; } = null!;

    public MiembroHogar()
    {
        Id = Guid.NewGuid();
    }
}
