using System.Linq.Expressions;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.LogicaNegocio.Usuarios;

public interface IUsuarioRepositorio
{
    void GuardarCambios();
    void Agregar(Usuario usuario);
    bool Existe(Expression<Func<Usuario, bool>> predicado);
    ObtenerUsuariosArgs ObtenerTodos(ParametroPaginacion? parametroPaginacion, ParametroUsuarioFiltro? parametroFiltro);
    void Eliminar(string email);
    Rol ObtenerRolPorId(Guid id);
    void Actualizar(Usuario usuario);
    Usuario ObtenerPorId(Guid id);
    Usuario ObtenerPorEmail(string email);
}
