using System.Linq.Expressions;
using SmartHome.LogicaNegocio;
using SmartHome.LogicaNegocio.Empresas;
using SmartHome.LogicaNegocio.Empresas.Entidades;

namespace SmartHome.Persistencia;

public class EmpresaRepositorio(ContextoSql contexto)
    : IEmpresaRepositorio
{
    private readonly ContextoSql _contexto = contexto;

    public void GuardarCambios()
    {
        _contexto.SaveChanges();
    }

    public void Agregar(Empresa empresa)
    {
        _contexto.Empresas.Add(empresa);
    }

    public ObtenerEmpresasArgs ObtenerTodos(ParametroPaginacion? parametroPaginacion,
        ParametroEmpresaFiltro? parametroFiltro)
    {
        var query = _contexto.Empresas.AsQueryable();

        parametroPaginacion ??= new ParametroPaginacion(1, 10);

        if (parametroFiltro != null)
        {
            if (!string.IsNullOrEmpty(parametroFiltro.Nombre))
            {
                query = query.Where(e => e.Nombre.ToLower() == parametroFiltro.Nombre.ToLower());
            }

            if (!string.IsNullOrEmpty(parametroFiltro.NombreCompletoCreador))
            {
                query = query.Where(e => e.NombreCreador.ToLower() == parametroFiltro.NombreCompletoCreador.ToLower());
            }
        }

        var totalEmpresas = query.Count();

        var cantidadPaginas = (totalEmpresas + parametroPaginacion.TamañoDePagina - 1) / parametroPaginacion.TamañoDePagina;

        var empresas = query
            .Skip((parametroPaginacion.NumeroDePagina - 1) * parametroPaginacion.TamañoDePagina)
            .Take(parametroPaginacion.TamañoDePagina)
            .ToList();

        return new ObtenerEmpresasArgs(empresas, cantidadPaginas);
    }

    public Empresa ObtenerPorId(Guid id)
    {
        return _contexto.Empresas.FirstOrDefault(x => x.Id == id);
    }

    public bool Existe(Expression<Func<Empresa, bool>> predicado)
    {
        return _contexto.Empresas.Any(predicado);
    }
}
