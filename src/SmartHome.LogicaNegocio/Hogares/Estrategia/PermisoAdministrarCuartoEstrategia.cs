using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.LogicaNegocio.Hogares.Estrategia;

public class PermisoAdministrarCuartoEstrategia : IPermisoEstrategia
{
    public bool TienePermiso(Usuario usuario, string idHogar, IHogarLogica logicaHogar)
    {
        return logicaHogar.EsDueñoHogar(usuario, idHogar) ||
               logicaHogar.TienePermisoAdministrarCuartos(usuario, idHogar);
    }
}
