using SmartHome.LogicaNegocio.Cuartos.Entidades;
using SmartHome.LogicaNegocio.Usuarios.Entidades;

namespace SmartHome.LogicaNegocio.Cuartos;
public interface ICuartoLogica
{
    void GuardarCambios();
    Cuarto Agregar(CrearCuartosArgs args, Usuario usuario);
    Cuarto ObtenerPorId(string id);
}
