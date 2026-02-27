using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.LogicaNegocio.Hogares.Estrategia;

public class PermisoModificarNombreDispositivoEstrategia : IPermisoEstrategia
{
    public bool TienePermiso(Usuario usuario, string idHogar, IHogarLogica logicaHogar)
    {
        return logicaHogar.EsDueñoHogar(usuario, idHogar) ||
               logicaHogar.TienePermisoModificarNombreDispositivos(usuario, idHogar);
    }
}
