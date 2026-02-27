using System.Linq.Expressions;
using SmartHome.LogicaNegocio.Empresas.Entidades;

namespace SmartHome.LogicaNegocio.Empresas;

public interface IEmpresaRepositorio
{
    void Agregar(Empresa empresa);
    void GuardarCambios();
    Empresa ObtenerPorId(Guid id);
    ObtenerEmpresasArgs ObtenerTodos(ParametroPaginacion? parametroPaginacion,
        ParametroEmpresaFiltro? parametroFiltro);
    bool Existe(Expression<Func<Empresa, bool>> predicado);
}
