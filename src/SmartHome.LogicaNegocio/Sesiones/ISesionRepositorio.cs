using SmartHome.LogicaNegocio.Sesiones.Entidades;

namespace SmartHome.LogicaNegocio.Sesiones;

public interface ISesionRepositorio
{
    void Agregar(Sesion sesion);
    void GuardarCambios();
    List<Sesion> ObtenerTodos();
    void Eliminar(string token);
}
