using SmartHome.LogicaNegocio.Hogares.Entidades;
using SmartHome.LogicaNegocio.Usuarios;

namespace SmartHome.WebApi.Controllers.Hogares.Modelos;

public class InformacionRespuestaListarMiembros
{
    public string NombreCompleto { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string FotoPerfil { get; init; } = null!;
    public bool TienePermisoListarDispositivos { get; init; }
    public bool TienePermisoAsociarDispositivos { get; init; }
    public bool RecibeNotificaciones { get; init; }
    public bool TienePermisoAdministrarCuartos { get; init; }
    public bool TienePermisoModificarNombreDispositivos { get; init; }

    public InformacionRespuestaListarMiembros(IUsuarioLogica usuarioLogica, MiembroHogar miembro)
    {
        var usuario = usuarioLogica.ObtenerUsuarioPorId(miembro.MiembroId);

        NombreCompleto = usuario.Nombre + " " + usuario.Apellido;
        Email = usuario.Email;
        FotoPerfil = usuario.FotoPerfil;
        TienePermisoListarDispositivos = miembro.PermisoListarDispositivos;
        TienePermisoAsociarDispositivos = miembro.PermisoAsociarDispositivos;
        RecibeNotificaciones = miembro.PermisoNotificaciones;
        TienePermisoAdministrarCuartos = miembro.PermisoAdministrarCuartos;
        TienePermisoModificarNombreDispositivos = miembro.PermisoModificarNombreDispositivos;
    }
}
