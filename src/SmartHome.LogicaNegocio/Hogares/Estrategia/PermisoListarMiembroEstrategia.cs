using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.LogicaNegocio.Hogares.Estrategia;

public class PermisoListarMiembroEstrategia : IPermisoEstrategia
{
    public bool TienePermiso(Usuario usuario, string idHogar, IHogarLogica logicaHogar)
    {
        return logicaHogar.EsDueñoHogar(usuario, idHogar);
    }
}
