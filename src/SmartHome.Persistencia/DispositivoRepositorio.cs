using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SmartHome.LogicaNegocio;
using SmartHome.LogicaNegocio.Dispositivos.Entidades;

namespace SmartHome.Persistencia;
public class DispositivoRepositorio(ContextoSql contexto)
    : IDispositivoRepositorio
{
    private readonly ContextoSql _contexto = contexto;

    public void GuardarCambios()
    {
        _contexto.SaveChanges();
    }

    public void Agregar(Dispositivo dispositivo)
    {
        _contexto.Dispositivos.Add(dispositivo);
    }

    public ObtenerDispositivosArgs ObtenerTodos(ParametroPaginacion? parametroPaginacion,
        ParametroDispositivoFiltro? parametroFiltro)
    {
        var query = _contexto.Dispositivos.Include(d => d.Fotografias).AsQueryable();

        parametroPaginacion ??= new ParametroPaginacion(1, 10);

        if (parametroFiltro != null)
        {
            if (!string.IsNullOrEmpty(parametroFiltro.NombreDispositivo))
            {
                query = query.Where(x => x.Nombre.ToLower() == parametroFiltro.NombreDispositivo.ToLower());
            }

            if (!string.IsNullOrEmpty(parametroFiltro.Modelo))
            {
                query = query.Where(x => x.Modelo.ToLower() == parametroFiltro.Modelo.ToLower());
            }

            if (!string.IsNullOrEmpty(parametroFiltro.NombreEmpresa))
            {
                var empresa = _contexto.Empresas.
                    FirstOrDefault(x => x.Nombre.ToLower() == parametroFiltro.NombreEmpresa.ToLower());

                query = empresa != null ? query.Where(x => x.EmpresaId == empresa.Id)
                    : query.Where(x => false);
            }

            if (!string.IsNullOrEmpty(parametroFiltro.TipoDispositivo))
            {
                query = Enum.TryParse<TipoDispositivo>(parametroFiltro.TipoDispositivo, true,
                    out var tipoDispositivoEnum)
                    ? query.Where(x => x.Tipo == tipoDispositivoEnum)
                    : query.Where(x => false);
            }
        }

        var totalDispositivos = query.Count();
        var cantidadPaginas = (totalDispositivos + parametroPaginacion.TamañoDePagina - 1) / parametroPaginacion.TamañoDePagina;
        var dispositivos = query
            .Skip((parametroPaginacion.NumeroDePagina - 1) * parametroPaginacion.TamañoDePagina)
            .Take(parametroPaginacion.TamañoDePagina)
            .ToList();

        return new ObtenerDispositivosArgs(dispositivos, cantidadPaginas);
    }

    public List<TipoDispositivo> ObtenerTiposDeDispositivos()
    {
        return Enum.GetValues<TipoDispositivo>().ToList();
    }

    public bool Existe(Expression<Func<Dispositivo, bool>> predicado)
    {
        return _contexto.Dispositivos.Any(predicado);
    }

    public Dispositivo ObtenerPorId(Guid id)
    {
        return _contexto.Dispositivos.FirstOrDefault(d => d.Id == id);
    }
}
