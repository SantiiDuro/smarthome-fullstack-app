using System.Linq.Expressions;
using SmartHome.LogicaNegocio;
using SmartHome.LogicaNegocio.Dispositivos.Entidades;

public interface IDispositivoRepositorio
{
    void GuardarCambios();
    void Agregar(Dispositivo dispositivo);
    bool Existe(Expression<Func<Dispositivo, bool>> predicado);
    Dispositivo ObtenerPorId(Guid id);
    List<TipoDispositivo> ObtenerTiposDeDispositivos();
    ObtenerDispositivosArgs ObtenerTodos(ParametroPaginacion? parametroPaginacion, ParametroDispositivoFiltro? parametroFiltro);
}
