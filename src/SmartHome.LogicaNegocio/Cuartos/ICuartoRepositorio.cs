using System.Linq.Expressions;
using SmartHome.LogicaNegocio.Cuartos.Entidades;

namespace SmartHome.LogicaNegocio.Cuartos;
public interface ICuartoRepositorio
{
    void GuardarCambios();
    void Agregar(Cuarto cuarto);
    bool Existe(Expression<Func<Cuarto, bool>> predicado);
    Cuarto ObtenerPorId(Guid id);
}
