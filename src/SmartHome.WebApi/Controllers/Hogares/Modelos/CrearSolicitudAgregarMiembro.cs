using SmartHome.LogicaNegocio.Hogares;
using SmartHome.LogicaNegocio.Hogares.Entidades;
using SmartHome.LogicaNegocio.Usuarios;

namespace SmartHome.WebApi.Controllers.Hogares.Modelos;

public class CrearSolicitudAgregarMiembro
{
    public string Email { get; init; } = null!;
    public bool PermisoAsociarDispositivos { get; init; }
    public bool PermisoListarDispositivos { get; init; }
    public bool PermisoNotificaciones { get; init; }
    public bool PerimsoAdministrarCuartos { get; init; }
    public bool PermisoModificarNombreDispositivos { get; init; }

    public MiembroHogar ObtenerMiembro(IUsuarioLogica logicaUsuario, IHogarLogica logicaHogar, string id)
    {
        if (string.IsNullOrEmpty(Email))
        {
            throw new ArgumentNullException(nameof(Email));
        }

        var usuario = logicaUsuario.ObtenerUsuarioPorEmail(Email);

        var hogar = logicaHogar.ObtenerPorId(id);

        return new MiembroHogar
        {
            Miembro = usuario,
            MiembroId = usuario.Id,
            PermisoAsociarDispositivos = PermisoAsociarDispositivos,
            PermisoListarDispositivos = PermisoListarDispositivos,
            PermisoNotificaciones = PermisoNotificaciones,
            PermisoAdministrarCuartos = PerimsoAdministrarCuartos,
            PermisoModificarNombreDispositivos = PermisoModificarNombreDispositivos,
            Hogar = hogar,
            HogarId = hogar.Id,
            Notificaciones = []
        };
    }
}
