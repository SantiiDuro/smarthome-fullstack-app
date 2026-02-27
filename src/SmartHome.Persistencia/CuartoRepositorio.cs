using System.Linq.Expressions;
using SmartHome.LogicaNegocio.Cuartos;
using SmartHome.LogicaNegocio.Cuartos.Entidades;

namespace SmartHome.Persistencia;
public class CuartoRepositorio(ContextoSql contexto)
    : ICuartoRepositorio
{
    private readonly ContextoSql _contexto = contexto;

    public void Agregar(Cuarto cuarto)
    {
        _contexto.Cuartos.Add(cuarto);
    }

    public bool Existe(Expression<Func<Cuarto, bool>> predicado)
    {
        return _contexto.Cuartos.Any(predicado);
    }

    public void GuardarCambios()
    {
        _contexto.SaveChanges();
    }

    public Cuarto ObtenerPorId(Guid id)
    {
        return _contexto.Cuartos.Find(id);
    }
}
