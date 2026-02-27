using System.Linq.Expressions;
using SmartHome.LogicaNegocio.Hogares.Entidades;

namespace SmartHome.LogicaNegocio.Hogares;
public interface IHogarRepositorio
{
    void GuardarCambios();
    void Agregar(Hogar hogar);
    List<Hogar> ObtenerTodos();
    void AgregarMiembro(MiembroHogar miembro);
    void ActualizarMiembro(MiembroHogar miembro);
    bool Existe(Expression<Func<Hogar, bool>> predicado);
    void Actualizar(Hogar hogar);
}
