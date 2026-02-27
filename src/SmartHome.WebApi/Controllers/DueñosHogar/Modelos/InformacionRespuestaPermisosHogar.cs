using SmartHome.LogicaNegocio.Hogares;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.WebApi.Controllers.DueñosHogar.Modelos;

public record class InformacionRespuestaPermisosHogar
{
    public bool PermisoAsociarDispositivos { get; init; }
    public bool PermisoListarDispositivos { get; init; }
    public bool PermisoAdministrarCuartos { get; init; }
    public bool PermisoModificarNombreDispositivos { get; init; }
    public bool PermisoAgregarMiembros { get; init; }
    public bool PermisoListarMiembros { get; init; }
    public bool PermisoModificarAlias { get; init; }

    public InformacionRespuestaPermisosHogar(Usuario usuario, string hogarId, IHogarLogica hogarLogica)
    {
        PermisoAsociarDispositivos = hogarLogica.TienePermisoAsociarDispositivo(usuario, hogarId) || hogarLogica.EsDueñoHogar(usuario, hogarId);
        PermisoListarDispositivos = hogarLogica.TienePermisoListarDispositivos(usuario, hogarId) || hogarLogica.EsDueñoHogar(usuario, hogarId);
        PermisoAdministrarCuartos = hogarLogica.TienePermisoAdministrarCuartos(usuario, hogarId) || hogarLogica.EsDueñoHogar(usuario, hogarId);
        PermisoModificarNombreDispositivos = hogarLogica.TienePermisoModificarNombreDispositivos(usuario, hogarId) || hogarLogica.EsDueñoHogar(usuario, hogarId);
        PermisoAgregarMiembros = hogarLogica.EsDueñoHogar(usuario, hogarId);
        PermisoListarMiembros = hogarLogica.EsDueñoHogar(usuario, hogarId);
        PermisoModificarAlias = hogarLogica.EsDueñoHogar(usuario, hogarId);
    }
}
