using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.LogicaNegocio.Hogares.Estrategia;

public interface IPermisoEstrategia
{
    bool TienePermiso(Usuario usuario, string idHogar, IHogarLogica logicaHogar);
}
