using SmartHome.LogicaNegocio.Empresas.Entidades;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.LogicaNegocio.Empresas;

public interface IEmpresaLogica
{
    Empresa Agregar(CrearEmpresasArgs args, Usuario usuario);
    void GuardarCambios();
    ObtenerEmpresasArgs ObtenerTodos(ParametroPaginacion? parametroPaginacion, ParametroEmpresaFiltro? parametroFiltro);
    Empresa ObtenerPorId(Guid id);
    List<string> ObtenerIdentificadoresDeImplementaciones();
}
